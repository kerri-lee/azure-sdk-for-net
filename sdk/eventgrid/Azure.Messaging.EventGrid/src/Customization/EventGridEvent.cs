// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.Core;

namespace Azure.Messaging.EventGrid.Models
{
    /// <summary> Properties of an event published to an Event Grid topic using the EventGrid Schema. </summary>
    public partial class EventGridEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventGridEvent"/> class.
        /// </summary>
        public EventGridEvent()
        {
        }

        /// <summary> Initializes a new instance of EventGridEvent. </summary>
        /// <param name="subject"> A resource path relative to the topic path. </param>
        /// <param name="data"> Event data specific to the event type. </param>
        /// <param name="eventType"> The type of the event that occurred. </param>
        /// <param name="dataVersion"> The schema version of the data object. </param>
        public EventGridEvent(string subject, BinaryData data, string eventType, string dataVersion)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }
            if (data.Equals(default(BinaryData)))
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (eventType == null)
            {
                throw new ArgumentNullException(nameof(eventType));
            }
            if (dataVersion == null)
            {
                throw new ArgumentNullException(nameof(dataVersion));
            }

            Subject = subject;
            Data = data;
            EventType = eventType;
            DataVersion = dataVersion;
        }

        /// <summary> Initializes a new instance of EventGridEvent. </summary>
        /// <param name="id"> An unique identifier for the event. </param>
        /// <param name="topic"> The resource path of the event source. </param>
        /// <param name="subject"> A resource path relative to the topic path. </param>
        /// <param name="data"> Event data specific to the event type. </param>
        /// <param name="eventType"> The type of the event that occurred. </param>
        /// <param name="eventTime"> The time (in UTC) the event was generated. </param>
        /// <param name="metadataVersion"> The schema version of the event metadata. </param>
        /// <param name="dataVersion"> The schema version of the data object. </param>
        internal EventGridEvent(string id, string topic, string subject, BinaryData data, string eventType, DateTimeOffset eventTime, string metadataVersion, string dataVersion)
        {
            Id = id;
            Topic = topic;
            Subject = subject;
            Data = data;
            EventType = eventType;
            EventTime = eventTime;
            MetadataVersion = metadataVersion;
            DataVersion = dataVersion;
        }

        /// <summary> An unique identifier for the event. </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary> The resource path of the event source. </summary>
        public string Topic { get; set; }
        /// <summary> A resource path relative to the topic path. </summary>
        public string Subject { get; set; }
        /// <summary> Event data specific to the event type. </summary>
        public BinaryData Data { get; set; }
        /// <summary> The type of the event that occurred. </summary>
        public string EventType { get; set; }
        /// <summary> The time (in UTC) the event was generated. </summary>
        public DateTimeOffset EventTime { get; set; } = DateTimeOffset.UtcNow;
        /// <summary> The schema version of the event metadata. </summary>
        public string MetadataVersion { get; set; }
        /// <summary> The schema version of the data object. </summary>
        public string DataVersion { get; set; }
        /// <summary> If event is a system event, describes the event type </summary>
        public Type DataType { get; internal set; }
    }
}
