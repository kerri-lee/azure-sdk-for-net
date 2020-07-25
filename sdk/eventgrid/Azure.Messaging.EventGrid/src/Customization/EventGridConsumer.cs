// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Messaging.EventGrid.Models;

namespace Azure.Messaging.EventGrid
{
    /// <summary>
    /// Class used to decode events from the Event Grid service.
    /// </summary>
    public class EventGridConsumer
    {
        /// <summary>
        /// Serializer used to decode events and custom payloads from JSON
        /// </summary>
        private ObjectSerializer _objectSerializer = new JsonObjectSerializer(new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        // Initialize some sort of dictionary for custom event types
        //private readonly ConcurrentDictionary<string, Type> _customEventTypeMappings = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventGridConsumer"/> class.
        /// </summary>
        public EventGridConsumer()
        {

        }

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in the EventGrid event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in the EventGrid event schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of EventGrid Events.</returns>
        public virtual EventGridEvent[] DeserializeEventGridEvents(string requestContent, CancellationToken cancellationToken = default)
        {
            // need to check if events are actually encoded in the eg schema
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));

            EventGridEventInternal[] egEvents = (EventGridEventInternal[])_objectSerializer.Deserialize(stream, typeof(EventGridEventInternal[]), cancellationToken);

            List<EventGridEvent> events = new List<EventGridEvent>();
            foreach (EventGridEventInternal egEvent in egEvents)
            {
                var stream1 = new MemoryStream();
                _objectSerializer.Serialize(stream1, egEvent.Data, egEvent.Data.GetType(), cancellationToken);
                stream1.Seek(0, SeekOrigin.Begin);

                SystemEventTypeMappingsTypes.SystemEventMappings.TryGetValue(egEvent.EventType, out Type dataType);
                events.Add(new EventGridEvent()
                {
                    Id = egEvent.Id,
                    Subject = egEvent.Subject,
                    Data = BinaryData.FromStream(stream1),
                    EventType = egEvent.EventType,
                    EventTime = egEvent.EventTime,
                    DataVersion = egEvent.DataVersion,
                    DataType = dataType
                });
            }
            return events.ToArray();
        }

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in the CloudEvent schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in the CloudEvent schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of CloudEvents.</returns>
        public virtual CloudEvent[] DeserializeCloudEvents(string requestContent, CancellationToken cancellationToken = default)
        {
            // need to check if events are actually encoded in the cloudevent schema

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));

            // note: need parameterless constructor generated
            CloudEventInternal[] cloudEvents = (CloudEventInternal[])_objectSerializer.Deserialize(stream, typeof(CloudEventInternal[]), cancellationToken);

            List<CloudEvent> events = new List<CloudEvent>();
            foreach (CloudEventInternal cloudEvent in cloudEvents)
            {
                var stream1 = new MemoryStream();
                if (!cloudEvent.DataBase64.Equals(default(BinaryData)))
                {
                    _objectSerializer.Serialize(stream1, cloudEvent.DataBase64, cloudEvent.DataBase64.GetType(), cancellationToken);
                    stream1.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    _objectSerializer.Serialize(stream1, cloudEvent.Data, cloudEvent.Data.GetType(), cancellationToken);
                    stream1.Seek(0, SeekOrigin.Begin);
                }

                SystemEventTypeMappingsTypes.SystemEventMappings.TryGetValue(cloudEvent.Type, out Type dataType);
                events.Add(new CloudEvent()
                {
                    Id = cloudEvent.Id,
                    Source = cloudEvent.Source,
                    Data = BinaryData.FromStream(stream1),
                    Type = cloudEvent.Type,
                    Time = cloudEvent.Time,
                    SpecVersion = cloudEvent.Specversion,
                    DataSchema = cloudEvent.Dataschema,
                    DataContentType = cloudEvent.Datacontenttype,
                    Subject = cloudEvent.Subject,
                    DataType = dataType
                });
            }
            return events.ToArray();
        }

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in a custom event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in a custom event schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of CloudEvents.</returns>
        public virtual T[] DeserializeCustomEvents<T>(string requestContent, CancellationToken cancellationToken = default)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));
            return (T[])_objectSerializer.Deserialize(stream, typeof(T[]), cancellationToken);
        }
    }
}
