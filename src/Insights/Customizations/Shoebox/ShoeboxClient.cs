﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Insights.Models;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace Microsoft.Azure.Insights
{
    internal static class ShoeboxClient
    {
        internal static int MaxParallelRequestsByName = 4;

        /// <summary>
        /// Retrieves the metric values from the shoebox
        /// </summary>
        /// <param name="filter">The $filter query string</param>
        /// <param name="location">The MetricLocation object</param>
        /// <returns>The MetricValueListResponse</returns>
        // Note: Does not populate Metric fields unrelated to query (i.e. "display name", resourceUri, and properties)
        internal static MetricListResponse GetMetricsInternal(MetricFilter filter, MetricLocation location)
        {
            return GetMetricsInternalAsync(filter, location).Result;
        }

        /// <summary>
        /// Retrieves the metric values from the shoebox
        /// </summary>
        /// <param name="filter">The $filter query string</param>
        /// <param name="location">The MetricLocation object</param>
        /// <returns>The MetricValueListResponse</returns>
        // Note: Does not populate Metric fields unrelated to query (i.e. "display name", resourceUri, and properties)
        internal static async Task<MetricListResponse> GetMetricsInternalAsync(MetricFilter filter, MetricLocation location)
        {
            // If metrics are requested by name, get those metrics specifically, unless too many are requested.
            // If no names or too many names are provided, get all metrics and filter if necessary.
            return new MetricListResponse()
            {
                MetricCollection = await (filter.Names == null || filter.Names.Count() > MaxParallelRequestsByName
                    ? GetMetricsByTimestampAsync(filter, location)
                    : GetMetricsByNameAsync(filter, location))
            };
        }

        // Generates queries for each metric by name (name-timestamp rowKey format) at collects the results
        // Note: Does not populate Metric fields unrelated to query (i.e. "display name", resourceUri, and properties)
        private static async Task<MetricCollection> GetMetricsByNameAsync(MetricFilter filter, MetricLocation location)
        {
            // Create a query for each metric name
            Dictionary<string, TableQuery> queries = GenerateMetricNameQueries(filter.Names, location.PartitionKey,
                filter.StartTime, filter.EndTime);

            // Execute the queries in parallel. Each query will correspond to one metric
            IList<Metric> metrics =
                await
                    Task.Factory.ContinueWhenAll(
                        queries.Select(async kvp => await GetMetricAsync(kvp.Key, kvp.Value, filter, location)).ToArray(),
                        tasks =>
                            new List<Metric>(tasks.Select(t => t.Result)));

            // Wrap metrics in MetricCollectionObject
            return new MetricCollection()
            {
                Value = metrics
            };
        }

        // Generates queries for all metrics by timestamp (timestamp-name rowKey format) and filters the results to the requested metrics (if any)
        // Note: Does not populate Metric fields unrelated to query (i.e. "display name", resourceUri, and properties)
        private static async Task<MetricCollection> GetMetricsByTimestampAsync(MetricFilter filter, MetricLocation location)
        {
            // Get all metric entities for timerange and group by metric name (second half of row key)
            IEnumerable<IGrouping<string, DynamicTableEntity>> groups =
                (await
                    GetEntitiesAsync(GetNdayTables(filter, location),
                        GenerateMetricTimestampQuery(location.PartitionKey, filter.StartTime, filter.EndTime))).GroupBy(
                            entity => entity.RowKey.Substring(entity.RowKey.LastIndexOf('_') + 1));

            // if names are specified, filter the results to those metrics only
            if (filter.Names != null)
            {
                groups = groups.Where(g => filter.Names.Select(ShoeboxHelper.TrimAndEscapeStorageKey).Contains(g.Key));
            }

            // Construct MetricCollection (list of metrics) by taking each group and converting the entities in that group to MetricValue objects
            return new MetricCollection()
            {
                Value =
                    groups.Select(g => new Metric()
                    {
                        Name = new LocalizableString()
                        {
                            Value = FindMetricName(g.Key, filter.Names)
                        },
                        StartTime = filter.StartTime,
                        EndTime = filter.EndTime,
                        TimeGrain = filter.TimeGrain,
                        MetricValues = g.Select(ResolveMetricEntity).ToList()
                    }).ToList()
            };
        }

        // This method tries to figure out the original name of the metric from the encoded name
        // Note: Will unescape the name if it is not in the list, but it will not be able to unhash it if it was hashed
        private static string FindMetricName(string encodedName, IEnumerable<string> names)
        {
            return
                names.FirstOrDefault(
                    n =>
                        string.Equals(ShoeboxHelper.TrimAndEscapeStorageKey(n), encodedName, StringComparison.OrdinalIgnoreCase)) ??
                ShoeboxHelper.UnEscapeStorageKey(encodedName);
        }

        // Gets the named metric by calling the provided query on each table that overlaps the given time range
        // Note: Does not populate Metric fields unrelated to query (i.e. "display name", resourceUri, and properties)
        private static async Task<Metric> GetMetricAsync(string name, TableQuery query, MetricFilter filter, MetricLocation location)
        {
            // The GetEnititesAsync function provides one task that will call all the queries in parallel
            return new Metric()
            {
                Name = new LocalizableString()
                {
                    Value = name
                },
                StartTime = filter.StartTime,
                EndTime = filter.EndTime,
                TimeGrain = filter.TimeGrain,
                MetricValues = (await GetEntitiesAsync(GetNdayTables(filter, location), query)).Select(ResolveMetricEntity).ToList()
            };
        }

        // Executes a table query and converts the resulting entities to MetricValues
        private static async Task<IList<DynamicTableEntity>> GetEntitiesAsync(CloudTable table, TableQuery query)
        {
            List<DynamicTableEntity> results = new List<DynamicTableEntity>();

            TableQuerySegment<DynamicTableEntity> resultSegment;
            TableContinuationToken continuationToken = null;

            // Table query returns a max of 1000 entities at a time, so continuation tokens must be followed
            do
            {
                resultSegment = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                results.AddRange(resultSegment.Results);
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);

            return results;
        }

        // Gets the entities from multiple queries and collects the results
        private static async Task<IList<DynamicTableEntity>> GetEntitiesAsync(IEnumerable<CloudTable> tables, TableQuery query)
        {
            return await Task.Factory.ContinueWhenAll<IList<DynamicTableEntity>, IList<DynamicTableEntity>>(
                tables.Select(table => GetEntitiesAsync(table, query)).ToArray(),
                tasks =>
                    tasks.Aggregate<Task<IList<DynamicTableEntity>>, IEnumerable<DynamicTableEntity>>(
                        new List<DynamicTableEntity>(), (list, t) => list.Union(t.Result)).ToList());
        }

        private static IEnumerable<CloudTable> GetNdayTables(MetricFilter filter, MetricLocation location)
        {
            return
                location.TableInfo.Where(info => info.StartTime < filter.EndTime && info.EndTime > filter.StartTime)
                    .Select(info => new CloudTableClient(new Uri(location.TableEndpoint), new StorageCredentials(info.SasToken)).GetTableReference(info.TableName));
        }

        // Creates a TableQuery for each named metric and return a dictionary mapping each name to its query
        // Note: The overall start and end times are used in each query since this reduces processing and the query will still work the same on each Nday table
        private static Dictionary<string, TableQuery> GenerateMetricNameQueries(IEnumerable<string> names, string partitionKey, DateTime startTime, DateTime endTime)
        {
            return names.ToDictionary(name => ShoeboxHelper.TrimAndEscapeStorageKey(name) + "__").ToDictionary(kvp => kvp.Value, kvp =>
                GenerateMetricQuery(
                partitionKey,
                kvp.Key + (DateTime.MaxValue.Ticks - endTime.Ticks).ToString("D19"),
                kvp.Key + (DateTime.MaxValue.Ticks - startTime.Ticks).ToString("D19")));
        }

        // Creates a TableQuery for getting metrics by timestamp
        private static TableQuery GenerateMetricTimestampQuery(string partitionKey, DateTime startTime, DateTime endTime)
        {
            return GenerateMetricQuery(
                partitionKey,
                (DateTime.MaxValue.Ticks - endTime.Ticks + 1).ToString("D19") + "__",
                (DateTime.MaxValue.Ticks - startTime.Ticks).ToString("D19") + "__");
        }

        // Creates a TableQuery object which filters entities to a particular partition key and the given row key range
        private static TableQuery GenerateMetricQuery(string partitionKey, string startRowKey, string endRowKey)
        {
            string partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            string rowStartFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, startRowKey);
            string rowEndFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, endRowKey);
            
            return new TableQuery()
            {
                FilterString = TableQuery.CombineFilters(partitionFilter, TableOperators.And, TableQuery.CombineFilters(rowStartFilter, TableOperators.And, rowEndFilter))
            };
        }

        // Converts a TableEntity to a MetricValue object
        private static MetricValue ResolveMetricEntity(DynamicTableEntity entity)
        {
            Dictionary<string, string> otherProperties = new Dictionary<string, string>();
            MetricValue metricValue = new MetricValue()
            {
                Timestamp = entity.Timestamp.UtcDateTime
            };

            foreach (string key in entity.Properties.Keys)
            {
                switch (key)
                {
                    case "Average":
                        metricValue.Average = entity[key].DoubleValue;
                        break;
                    case "Minimum":
                        metricValue.Minimum = entity[key].DoubleValue;
                        break;
                    case "Maximum":
                        metricValue.Maximum = entity[key].DoubleValue;
                        break;
                    case "Total":
                        metricValue.Total = entity[key].DoubleValue;
                        break;
                    case "Count":
                        metricValue.Count = entity[key].Int64Value;
                        break;
                    case "Last":
                        metricValue.Last = entity[key].DoubleValue;
                        break;
                    default:
                        otherProperties.Add(key, entity[key].ToString());
                        break;
                }
            }

            metricValue.Properties = otherProperties;
            return metricValue;
        }
    }
}