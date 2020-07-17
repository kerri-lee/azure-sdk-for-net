// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Messaging.EventGrid.Models
{
    /// <summary> The request that generated the event. </summary>
    public partial class ContainerRegistryEventRequest
    {
        /// <summary> Initializes a new instance of ContainerRegistryEventRequest. </summary>
        internal ContainerRegistryEventRequest()
        {
        }

        /// <summary> Initializes a new instance of ContainerRegistryEventRequest. </summary>
        /// <param name="id"> The ID of the request that initiated the event. </param>
        /// <param name="addr"> The IP or hostname and possibly port of the client connection that initiated the event. This is the RemoteAddr from the standard http request. </param>
        /// <param name="host"> The externally accessible hostname of the registry instance, as specified by the http host header on incoming requests. </param>
        /// <param name="method"> The request method that generated the event. </param>
        /// <param name="useragent"> The user agent header of the request. </param>
        internal ContainerRegistryEventRequest(string id, string addr, string host, string method, string useragent)
        {
            Id = id;
            Addr = addr;
            Host = host;
            Method = method;
            Useragent = useragent;
        }

        /// <summary> The ID of the request that initiated the event. </summary>
        public string Id { get; }
        /// <summary> The IP or hostname and possibly port of the client connection that initiated the event. This is the RemoteAddr from the standard http request. </summary>
        public string Addr { get; }
        /// <summary> The externally accessible hostname of the registry instance, as specified by the http host header on incoming requests. </summary>
        public string Host { get; }
        /// <summary> The request method that generated the event. </summary>
        public string Method { get; }
        /// <summary> The user agent header of the request. </summary>
        public string Useragent { get; }
    }
}
