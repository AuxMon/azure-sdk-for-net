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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hyak.Common;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Management.Compute
{
    /// <summary>
    /// Operations for managing the virtual machine extension images in compute
    /// management.
    /// </summary>
    internal partial class VirtualMachineExtensionImageOperations : IServiceOperations<ComputeManagementClient>, IVirtualMachineExtensionImageOperations
    {
        /// <summary>
        /// Initializes a new instance of the
        /// VirtualMachineExtensionImageOperations class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the service client.
        /// </param>
        internal VirtualMachineExtensionImageOperations(ComputeManagementClient client)
        {
            this._client = client;
        }
        
        private ComputeManagementClient _client;
        
        /// <summary>
        /// Gets a reference to the
        /// Microsoft.Azure.Management.Compute.ComputeManagementClient.
        /// </summary>
        public ComputeManagementClient Client
        {
            get { return this._client; }
        }
        
        /// <summary>
        /// Gets a virtual machine extension image.
        /// </summary>
        /// <param name='parameters'>
        /// Optional.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The get virtual machine extension image operation response.
        /// </returns>
        public async Task<VirtualMachineExtensionImageGetResponse> GetAsync(VirtualMachineExtensionImageGetParameters parameters, CancellationToken cancellationToken)
        {
            // Validate
            if (parameters != null)
            {
                if (parameters.Location == null)
                {
                    throw new ArgumentNullException("parameters.Location");
                }
                if (parameters.PublisherName == null)
                {
                    throw new ArgumentNullException("parameters.PublisherName");
                }
                if (parameters.Type == null)
                {
                    throw new ArgumentNullException("parameters.Type");
                }
                if (parameters.Version == null)
                {
                    throw new ArgumentNullException("parameters.Version");
                }
            }
            
            // Tracing
            bool shouldTrace = TracingAdapter.IsEnabled;
            string invocationId = null;
            if (shouldTrace)
            {
                invocationId = TracingAdapter.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("parameters", parameters);
                TracingAdapter.Enter(invocationId, this, "GetAsync", tracingParameters);
            }
            
            // Construct URL
            string url = "";
            url = url + "/subscriptions/";
            if (this.Client.Credentials.SubscriptionId != null)
            {
                url = url + Uri.EscapeDataString(this.Client.Credentials.SubscriptionId);
            }
            url = url + "/providers/";
            url = url + "Microsoft.Compute";
            url = url + "/locations/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.Location);
            }
            url = url + "/publishers/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.PublisherName);
            }
            url = url + "/artifacttypes/vmextension/types/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.Type);
            }
            url = url + "/versions/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.Version);
            }
            List<string> queryParameters = new List<string>();
            queryParameters.Add("api-version=2015-06-15");
            if (queryParameters.Count > 0)
            {
                url = url + "?" + string.Join("&", queryParameters);
            }
            string baseUrl = this.Client.BaseUri.AbsoluteUri;
            // Trim '/' character from the end of baseUrl and beginning of url.
            if (baseUrl[baseUrl.Length - 1] == '/')
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            url = baseUrl + "/" + url;
            url = url.Replace(" ", "%20");
            
            // Create HTTP transport objects
            HttpRequestMessage httpRequest = null;
            try
            {
                httpRequest = new HttpRequestMessage();
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = new Uri(url);
                
                // Set Headers
                
                // Set Credentials
                cancellationToken.ThrowIfCancellationRequested();
                await this.Client.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                
                // Send Request
                HttpResponseMessage httpResponse = null;
                try
                {
                    if (shouldTrace)
                    {
                        TracingAdapter.SendRequest(invocationId, httpRequest);
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    httpResponse = await this.Client.HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                    if (shouldTrace)
                    {
                        TracingAdapter.ReceiveResponse(invocationId, httpResponse);
                    }
                    HttpStatusCode statusCode = httpResponse.StatusCode;
                    if (statusCode != HttpStatusCode.OK)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        CloudException ex = CloudException.Create(httpRequest, null, httpResponse, await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                        if (shouldTrace)
                        {
                            TracingAdapter.Error(invocationId, ex);
                        }
                        throw ex;
                    }
                    
                    // Create Result
                    VirtualMachineExtensionImageGetResponse result = null;
                    // Deserialize Response
                    if (statusCode == HttpStatusCode.OK)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        string responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        result = new VirtualMachineExtensionImageGetResponse();
                        JToken responseDoc = null;
                        if (string.IsNullOrEmpty(responseContent) == false)
                        {
                            responseDoc = JToken.Parse(responseContent);
                        }
                        
                        if (responseDoc != null && responseDoc.Type != JTokenType.Null)
                        {
                            VirtualMachineExtensionImage virtualMachineExtensionImageInstance = new VirtualMachineExtensionImage();
                            result.VirtualMachineExtensionImage = virtualMachineExtensionImageInstance;
                            
                            JToken propertiesValue = responseDoc["properties"];
                            if (propertiesValue != null && propertiesValue.Type != JTokenType.Null)
                            {
                                JToken operatingSystemValue = propertiesValue["operatingSystem"];
                                if (operatingSystemValue != null && operatingSystemValue.Type != JTokenType.Null)
                                {
                                    string operatingSystemInstance = ((string)operatingSystemValue);
                                    virtualMachineExtensionImageInstance.OperatingSystem = operatingSystemInstance;
                                }
                                
                                JToken computeRoleValue = propertiesValue["computeRole"];
                                if (computeRoleValue != null && computeRoleValue.Type != JTokenType.Null)
                                {
                                    string computeRoleInstance = ((string)computeRoleValue);
                                    virtualMachineExtensionImageInstance.ComputeRole = computeRoleInstance;
                                }
                                
                                JToken handlerSchemaValue = propertiesValue["handlerSchema"];
                                if (handlerSchemaValue != null && handlerSchemaValue.Type != JTokenType.Null)
                                {
                                    string handlerSchemaInstance = ((string)handlerSchemaValue);
                                    virtualMachineExtensionImageInstance.HandlerSchema = handlerSchemaInstance;
                                }
                                
                                JToken vmScaleSetEnabledValue = propertiesValue["vmScaleSetEnabled"];
                                if (vmScaleSetEnabledValue != null && vmScaleSetEnabledValue.Type != JTokenType.Null)
                                {
                                    bool vmScaleSetEnabledInstance = ((bool)vmScaleSetEnabledValue);
                                    virtualMachineExtensionImageInstance.VMScaleSetEnabled = vmScaleSetEnabledInstance;
                                }
                                
                                JToken supportsMultipleExtensionsValue = propertiesValue["supportsMultipleExtensions"];
                                if (supportsMultipleExtensionsValue != null && supportsMultipleExtensionsValue.Type != JTokenType.Null)
                                {
                                    bool supportsMultipleExtensionsInstance = ((bool)supportsMultipleExtensionsValue);
                                    virtualMachineExtensionImageInstance.SupportsMultipleExtensions = supportsMultipleExtensionsInstance;
                                }
                            }
                            
                            JToken idValue = responseDoc["id"];
                            if (idValue != null && idValue.Type != JTokenType.Null)
                            {
                                string idInstance = ((string)idValue);
                                virtualMachineExtensionImageInstance.Id = idInstance;
                            }
                            
                            JToken nameValue = responseDoc["name"];
                            if (nameValue != null && nameValue.Type != JTokenType.Null)
                            {
                                string nameInstance = ((string)nameValue);
                                virtualMachineExtensionImageInstance.Name = nameInstance;
                            }
                            
                            JToken locationValue = responseDoc["location"];
                            if (locationValue != null && locationValue.Type != JTokenType.Null)
                            {
                                string locationInstance = ((string)locationValue);
                                virtualMachineExtensionImageInstance.Location = locationInstance;
                            }
                        }
                        
                    }
                    result.StatusCode = statusCode;
                    if (httpResponse.Headers.Contains("x-ms-request-id"))
                    {
                        result.RequestId = httpResponse.Headers.GetValues("x-ms-request-id").FirstOrDefault();
                    }
                    
                    if (shouldTrace)
                    {
                        TracingAdapter.Exit(invocationId, result);
                    }
                    return result;
                }
                finally
                {
                    if (httpResponse != null)
                    {
                        httpResponse.Dispose();
                    }
                }
            }
            finally
            {
                if (httpRequest != null)
                {
                    httpRequest.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Gets a list of virtual machine extension image types.
        /// </summary>
        /// <param name='parameters'>
        /// Optional.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// A list of virtual machine image resource information.
        /// </returns>
        public async Task<VirtualMachineImageResourceList> ListTypesAsync(VirtualMachineExtensionImageListTypesParameters parameters, CancellationToken cancellationToken)
        {
            // Validate
            if (parameters != null)
            {
                if (parameters.Location == null)
                {
                    throw new ArgumentNullException("parameters.Location");
                }
                if (parameters.PublisherName == null)
                {
                    throw new ArgumentNullException("parameters.PublisherName");
                }
            }
            
            // Tracing
            bool shouldTrace = TracingAdapter.IsEnabled;
            string invocationId = null;
            if (shouldTrace)
            {
                invocationId = TracingAdapter.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("parameters", parameters);
                TracingAdapter.Enter(invocationId, this, "ListTypesAsync", tracingParameters);
            }
            
            // Construct URL
            string url = "";
            url = url + "/subscriptions/";
            if (this.Client.Credentials.SubscriptionId != null)
            {
                url = url + Uri.EscapeDataString(this.Client.Credentials.SubscriptionId);
            }
            url = url + "/providers/";
            url = url + "Microsoft.Compute";
            url = url + "/locations/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.Location);
            }
            url = url + "/publishers/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.PublisherName);
            }
            url = url + "/artifacttypes/vmextension/types";
            List<string> queryParameters = new List<string>();
            queryParameters.Add("api-version=2015-06-15");
            if (queryParameters.Count > 0)
            {
                url = url + "?" + string.Join("&", queryParameters);
            }
            string baseUrl = this.Client.BaseUri.AbsoluteUri;
            // Trim '/' character from the end of baseUrl and beginning of url.
            if (baseUrl[baseUrl.Length - 1] == '/')
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            url = baseUrl + "/" + url;
            url = url.Replace(" ", "%20");
            
            // Create HTTP transport objects
            HttpRequestMessage httpRequest = null;
            try
            {
                httpRequest = new HttpRequestMessage();
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = new Uri(url);
                
                // Set Headers
                
                // Set Credentials
                cancellationToken.ThrowIfCancellationRequested();
                await this.Client.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                
                // Send Request
                HttpResponseMessage httpResponse = null;
                try
                {
                    if (shouldTrace)
                    {
                        TracingAdapter.SendRequest(invocationId, httpRequest);
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    httpResponse = await this.Client.HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                    if (shouldTrace)
                    {
                        TracingAdapter.ReceiveResponse(invocationId, httpResponse);
                    }
                    HttpStatusCode statusCode = httpResponse.StatusCode;
                    if (statusCode != HttpStatusCode.OK)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        CloudException ex = CloudException.Create(httpRequest, null, httpResponse, await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                        if (shouldTrace)
                        {
                            TracingAdapter.Error(invocationId, ex);
                        }
                        throw ex;
                    }
                    
                    // Create Result
                    VirtualMachineImageResourceList result = null;
                    // Deserialize Response
                    if (statusCode == HttpStatusCode.OK)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        string responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        result = new VirtualMachineImageResourceList();
                        JToken responseDoc = null;
                        if (string.IsNullOrEmpty(responseContent) == false)
                        {
                            responseDoc = JToken.Parse(responseContent);
                        }
                        
                        if (responseDoc != null && responseDoc.Type != JTokenType.Null)
                        {
                            JToken resourcesArray = responseDoc;
                            if (resourcesArray != null && resourcesArray.Type != JTokenType.Null)
                            {
                                foreach (JToken resourcesValue in ((JArray)resourcesArray))
                                {
                                    VirtualMachineImageResource virtualMachineImageResourceInstance = new VirtualMachineImageResource();
                                    result.Resources.Add(virtualMachineImageResourceInstance);
                                    
                                    JToken idValue = resourcesValue["id"];
                                    if (idValue != null && idValue.Type != JTokenType.Null)
                                    {
                                        string idInstance = ((string)idValue);
                                        virtualMachineImageResourceInstance.Id = idInstance;
                                    }
                                    
                                    JToken nameValue = resourcesValue["name"];
                                    if (nameValue != null && nameValue.Type != JTokenType.Null)
                                    {
                                        string nameInstance = ((string)nameValue);
                                        virtualMachineImageResourceInstance.Name = nameInstance;
                                    }
                                    
                                    JToken locationValue = resourcesValue["location"];
                                    if (locationValue != null && locationValue.Type != JTokenType.Null)
                                    {
                                        string locationInstance = ((string)locationValue);
                                        virtualMachineImageResourceInstance.Location = locationInstance;
                                    }
                                }
                            }
                        }
                        
                    }
                    result.StatusCode = statusCode;
                    if (httpResponse.Headers.Contains("x-ms-request-id"))
                    {
                        result.RequestId = httpResponse.Headers.GetValues("x-ms-request-id").FirstOrDefault();
                    }
                    
                    if (shouldTrace)
                    {
                        TracingAdapter.Exit(invocationId, result);
                    }
                    return result;
                }
                finally
                {
                    if (httpResponse != null)
                    {
                        httpResponse.Dispose();
                    }
                }
            }
            finally
            {
                if (httpRequest != null)
                {
                    httpRequest.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Gets a list of virtual machine extension image versions.
        /// </summary>
        /// <param name='parameters'>
        /// Optional.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// A list of virtual machine image resource information.
        /// </returns>
        public async Task<VirtualMachineImageResourceList> ListVersionsAsync(VirtualMachineExtensionImageListVersionsParameters parameters, CancellationToken cancellationToken)
        {
            // Validate
            if (parameters != null)
            {
                if (parameters.Location == null)
                {
                    throw new ArgumentNullException("parameters.Location");
                }
                if (parameters.PublisherName == null)
                {
                    throw new ArgumentNullException("parameters.PublisherName");
                }
                if (parameters.Type == null)
                {
                    throw new ArgumentNullException("parameters.Type");
                }
            }
            
            // Tracing
            bool shouldTrace = TracingAdapter.IsEnabled;
            string invocationId = null;
            if (shouldTrace)
            {
                invocationId = TracingAdapter.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("parameters", parameters);
                TracingAdapter.Enter(invocationId, this, "ListVersionsAsync", tracingParameters);
            }
            
            // Construct URL
            string url = "";
            url = url + "/subscriptions/";
            if (this.Client.Credentials.SubscriptionId != null)
            {
                url = url + Uri.EscapeDataString(this.Client.Credentials.SubscriptionId);
            }
            url = url + "/providers/";
            url = url + "Microsoft.Compute";
            url = url + "/locations/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.Location);
            }
            url = url + "/publishers/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.PublisherName);
            }
            url = url + "/artifacttypes/vmextension/types/";
            if (parameters != null)
            {
                url = url + Uri.EscapeDataString(parameters.Type);
            }
            url = url + "/versions";
            List<string> queryParameters = new List<string>();
            queryParameters.Add("api-version=2015-06-15");
            if (parameters != null && parameters.FilterExpression != null)
            {
                queryParameters.Add(parameters.FilterExpression);
            }
            if (queryParameters.Count > 0)
            {
                url = url + "?" + string.Join("&", queryParameters);
            }
            string baseUrl = this.Client.BaseUri.AbsoluteUri;
            // Trim '/' character from the end of baseUrl and beginning of url.
            if (baseUrl[baseUrl.Length - 1] == '/')
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            url = baseUrl + "/" + url;
            url = url.Replace(" ", "%20");
            
            // Create HTTP transport objects
            HttpRequestMessage httpRequest = null;
            try
            {
                httpRequest = new HttpRequestMessage();
                httpRequest.Method = HttpMethod.Get;
                httpRequest.RequestUri = new Uri(url);
                
                // Set Headers
                
                // Set Credentials
                cancellationToken.ThrowIfCancellationRequested();
                await this.Client.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                
                // Send Request
                HttpResponseMessage httpResponse = null;
                try
                {
                    if (shouldTrace)
                    {
                        TracingAdapter.SendRequest(invocationId, httpRequest);
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    httpResponse = await this.Client.HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                    if (shouldTrace)
                    {
                        TracingAdapter.ReceiveResponse(invocationId, httpResponse);
                    }
                    HttpStatusCode statusCode = httpResponse.StatusCode;
                    if (statusCode != HttpStatusCode.OK)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        CloudException ex = CloudException.Create(httpRequest, null, httpResponse, await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                        if (shouldTrace)
                        {
                            TracingAdapter.Error(invocationId, ex);
                        }
                        throw ex;
                    }
                    
                    // Create Result
                    VirtualMachineImageResourceList result = null;
                    // Deserialize Response
                    if (statusCode == HttpStatusCode.OK)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        string responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        result = new VirtualMachineImageResourceList();
                        JToken responseDoc = null;
                        if (string.IsNullOrEmpty(responseContent) == false)
                        {
                            responseDoc = JToken.Parse(responseContent);
                        }
                        
                        if (responseDoc != null && responseDoc.Type != JTokenType.Null)
                        {
                            JToken resourcesArray = responseDoc;
                            if (resourcesArray != null && resourcesArray.Type != JTokenType.Null)
                            {
                                foreach (JToken resourcesValue in ((JArray)resourcesArray))
                                {
                                    VirtualMachineImageResource virtualMachineImageResourceInstance = new VirtualMachineImageResource();
                                    result.Resources.Add(virtualMachineImageResourceInstance);
                                    
                                    JToken idValue = resourcesValue["id"];
                                    if (idValue != null && idValue.Type != JTokenType.Null)
                                    {
                                        string idInstance = ((string)idValue);
                                        virtualMachineImageResourceInstance.Id = idInstance;
                                    }
                                    
                                    JToken nameValue = resourcesValue["name"];
                                    if (nameValue != null && nameValue.Type != JTokenType.Null)
                                    {
                                        string nameInstance = ((string)nameValue);
                                        virtualMachineImageResourceInstance.Name = nameInstance;
                                    }
                                    
                                    JToken locationValue = resourcesValue["location"];
                                    if (locationValue != null && locationValue.Type != JTokenType.Null)
                                    {
                                        string locationInstance = ((string)locationValue);
                                        virtualMachineImageResourceInstance.Location = locationInstance;
                                    }
                                }
                            }
                        }
                        
                    }
                    result.StatusCode = statusCode;
                    if (httpResponse.Headers.Contains("x-ms-request-id"))
                    {
                        result.RequestId = httpResponse.Headers.GetValues("x-ms-request-id").FirstOrDefault();
                    }
                    
                    if (shouldTrace)
                    {
                        TracingAdapter.Exit(invocationId, result);
                    }
                    return result;
                }
                finally
                {
                    if (httpResponse != null)
                    {
                        httpResponse.Dispose();
                    }
                }
            }
            finally
            {
                if (httpRequest != null)
                {
                    httpRequest.Dispose();
                }
            }
        }
    }
}
