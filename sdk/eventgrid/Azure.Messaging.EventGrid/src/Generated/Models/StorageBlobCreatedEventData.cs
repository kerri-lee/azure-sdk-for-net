// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Messaging.EventGrid.Models
{
    /// <summary> Schema of the Data property of an EventGridEvent for an Microsoft.Storage.BlobCreated event. </summary>
    public partial class StorageBlobCreatedEventData
    {
        /// <summary> Initializes a new instance of StorageBlobCreatedEventData. </summary>
        public StorageBlobCreatedEventData()
        {
        }

        /// <summary> Initializes a new instance of StorageBlobCreatedEventData. </summary>
        /// <param name="api"> The name of the API/operation that triggered this event. </param>
        /// <param name="clientRequestId"> A request id provided by the client of the storage API operation that triggered this event. </param>
        /// <param name="requestId"> The request id generated by the Storage service for the storage API operation that triggered this event. </param>
        /// <param name="eTag"> The etag of the blob at the time this event was triggered. </param>
        /// <param name="contentType"> The content type of the blob. This is the same as what would be returned in the Content-Type header from the blob. </param>
        /// <param name="contentLength"> The size of the blob in bytes. This is the same as what would be returned in the Content-Length header from the blob. </param>
        /// <param name="contentOffset"> The offset of the blob in bytes. </param>
        /// <param name="blobType"> The type of blob. </param>
        /// <param name="url"> The path to the blob. </param>
        /// <param name="sequencer"> An opaque string value representing the logical sequence of events for any particular blob name. Users can use standard string comparison to understand the relative sequence of two events on the same blob name. </param>
        /// <param name="identity"> The identity of the requester that triggered this event. </param>
        /// <param name="storageDiagnostics"> For service use only. Diagnostic data occasionally included by the Azure Storage service. This property should be ignored by event consumers. </param>
        internal StorageBlobCreatedEventData(string api, string clientRequestId, string requestId, string eTag, string contentType, long? contentLength, long? contentOffset, string blobType, string url, string sequencer, string identity, object storageDiagnostics)
        {
            Api = api;
            ClientRequestId = clientRequestId;
            RequestId = requestId;
            ETag = eTag;
            ContentType = contentType;
            ContentLength = contentLength;
            ContentOffset = contentOffset;
            BlobType = blobType;
            Url = url;
            Sequencer = sequencer;
            Identity = identity;
            StorageDiagnostics = storageDiagnostics;
        }

        /// <summary> The name of the API/operation that triggered this event. </summary>
        public string Api { get; set; }
        /// <summary> A request id provided by the client of the storage API operation that triggered this event. </summary>
        public string ClientRequestId { get; set; }
        /// <summary> The request id generated by the Storage service for the storage API operation that triggered this event. </summary>
        public string RequestId { get; set; }
        /// <summary> The etag of the blob at the time this event was triggered. </summary>
        public string ETag { get; set; }
        /// <summary> The content type of the blob. This is the same as what would be returned in the Content-Type header from the blob. </summary>
        public string ContentType { get; set; }
        /// <summary> The size of the blob in bytes. This is the same as what would be returned in the Content-Length header from the blob. </summary>
        public long? ContentLength { get; set; }
        /// <summary> The offset of the blob in bytes. </summary>
        public long? ContentOffset { get; set; }
        /// <summary> The type of blob. </summary>
        public string BlobType { get; set; }
        /// <summary> The path to the blob. </summary>
        public string Url { get; set; }
        /// <summary> An opaque string value representing the logical sequence of events for any particular blob name. Users can use standard string comparison to understand the relative sequence of two events on the same blob name. </summary>
        public string Sequencer { get; set; }
        /// <summary> The identity of the requester that triggered this event. </summary>
        public string Identity { get; set; }
        /// <summary> For service use only. Diagnostic data occasionally included by the Azure Storage service. This property should be ignored by event consumers. </summary>
        public object StorageDiagnostics { get; set; }
    }
}
