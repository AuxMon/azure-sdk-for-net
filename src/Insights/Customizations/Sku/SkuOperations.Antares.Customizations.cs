// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Insights.Models;

namespace Microsoft.Azure.Management.Insights
{
    /// <summary>
    /// Operations for managing resources sku.
    /// </summary>
    internal partial class AntaresSkuOperations
    {
        private static readonly Uri BaseUri = new Uri("http://localhost.com");
        private static readonly UriTemplate AntaresUriTemplate = new UriTemplate("/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/microsoft.web/serverFarms/{serverFarmName}", true);

        // Verify resourceId is of a supported type
        internal static bool IsAntaresResourceType(string resourceId)
        {
            return AntaresUriTemplate.Match(BaseUri, new Uri(BaseUri, resourceId)) != null;
        }

        internal static SkuListResponse ListAntaresSkus()
        {
            return new SkuListResponse
            {
                StatusCode = HttpStatusCode.OK,
                Value = new List<SkuDefinition>
                {
                    new SkuDefinition 
                    {
                        Sku =  new Sku
                        {
                            Name = "S1",
                            Tier = "Standard"
                        },
                        Capacity = new Capacity
                        {
                            Minimum = 1,
                            Maximum = 10,
                            Default = 1,
                            ScaleType = SupportedScaleType.Automatic
                        }
                    },
                    new SkuDefinition 
                    {
                        Sku = new Sku
                        {
                            Name = "S2",
                            Tier = "Standard",
                        },
                        Capacity = new Capacity
                        {
                            Minimum = 1,
                            Maximum = 10,
                            Default = 1,
                            ScaleType = SupportedScaleType.Automatic
                        }
                    },
                    new SkuDefinition 
                    {
                        Sku = new Sku
                        {
                            Name = "S3",
                            Tier = "Standard",
                            
                        },
                        Capacity = new Capacity
                        {
                            Minimum = 1,
                            Maximum = 10,
                            Default = 1,
                            ScaleType = SupportedScaleType.Automatic
                        }
                    },
                    new SkuDefinition 
                    {
                        Sku = new Sku
                        {
                            Name = "B1",
                            Tier = "Basic",
                            
                        },
                        Capacity = new Capacity
                        {
                            Minimum = 1,
                            Maximum = 3,
                            Default = 1,
                            ScaleType = SupportedScaleType.Manual
                        }
                    },
                    new SkuDefinition 
                    {
                        Sku = new Sku
                        {
                            Name = "B2",
                            Tier = "Basic",
                        },
                        Capacity = new Capacity
                        {
                            Minimum = 1,
                            Maximum = 3,
                            Default = 1,
                            ScaleType = SupportedScaleType.Manual
                        }
                    },
                    new SkuDefinition
                    {
                        Sku = new Sku
                        {
                            Name = "B3",
                            Tier = "Basic",
                            
                        },
                        Capacity = new Capacity
                        {
                            Minimum = 1,
                            Maximum = 3,
                            Default = 1,
                            ScaleType = SupportedScaleType.Manual
                        }
                    },
                    new SkuDefinition 
                    {
                        Sku = new Sku
                        {
                            Name = "D1",
                            Tier = "Shared",
                        },
                        Capacity = new Capacity
                        {
                            ScaleType = SupportedScaleType.None
                        }
                    },
                    new SkuDefinition 
                    {
                        Sku = new Sku
                        {
                            Name = "F1",
                            Tier = "Free",
                        },
                        Capacity = new Capacity
                        {
                            ScaleType = SupportedScaleType.None
                        }
                    }
                }
            };
        }

        internal async static Task<SkuGetResponse> GetAntaresCurrentSku(SkuOperations skuOperations, string resourceId, string apiVersion, CancellationToken cancellationToken)
        {
            AntaresSkuGetResponse response = await skuOperations.GetAntaresCurrentSkuInternalAsync(resourceId, apiVersion, cancellationToken);

            return new SkuGetResponse
            {
                Properties = new SkuGetProperties
                {
                    Sku = new CurrentSku
                    {
                        Name = AntaresSkuOperations.GetAnatresSkuName(response.Properties.CurrentWorkerSize, response.Properties.Sku),
                        Tier = response.Properties.Sku,
                        Capacity = response.Properties.CurrentNumberOfWorkers
                    }
                }
            };
        }

        internal static Task<SkuUpdateResponse> UpdateAntaresCurrentSkuAsync(
            SkuOperations skuOperations,
            string resourceId,
            SkuUpdateParameters parameters,
            string apiVersion,
            CancellationToken cancellationToken)
        {
            AntaresSkuUpdateRequest antaresUpdateParameters = new AntaresSkuUpdateRequest
            {
                WorkerSize = AntaresSkuOperations.GetAntaresWorkerSize(parameters.Sku.Name),
                Sku = parameters.Sku.Tier,
                NumberOfWorkers = parameters.Sku.Capacity
            };

            return skuOperations.UpdateAntaresCurrentSkuInternalAsync(resourceId, antaresUpdateParameters, apiVersion, cancellationToken);
        }

        private static int GetAntaresWorkerSize(string skuName)
        {
            switch (skuName)
            {
                case "S1":
                case "B1":
                case "D1":
                case "F1":
                    return 0;
                case "S2":
                case "B2":
                    return 1;
                case "S3":
                case "B3":
                    return 2;
                default:
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid SKU Name: {0}", skuName));
            }
        }

        private static string GetAnatresSkuName(int workerSize, string tier)
        {
            switch (workerSize)
            {
                case 0:
                    switch (tier)
                    {
                        case "Standard":
                            return "S1";
                        case "Basic":
                            return "B1";
                        case "Shared":
                            return "D1";
                        case "Free":
                            return "F1";
                        default:
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No SKU for tier {0} and worker size {1}", tier, workerSize));
                    }
                case 1:
                    switch (tier)
                    {
                        case "Standard":
                            return "S2";
                        case "Basic":
                            return "B2";
                        default:
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No SKU for tier {0} and worker size {1}", tier, workerSize));
                    }
                case 2:
                    switch (tier)
                    {
                        case "Standard":
                            return "S3";
                        case "Basic":
                            return "B3";
                        default:
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No SKU for tier {0} and worker size {1}", tier, workerSize));
                    }
                default:
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No SKU for tier {0} and worker size {1}", tier, workerSize));
            }
        }
    }
}