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
        public ObjectSerializer ObjectSerializer { get; set; } = new JsonObjectSerializer(new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true
        });

        // Initialize some sort of dictionary for custom event types
        private readonly ConcurrentDictionary<string, Type> _customEventTypeMappings = new ConcurrentDictionary<string, Type>();

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
        /// The JSON encoded representation of either a single event or an array or events, encoded in the EventGrid schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of EventGrid Events.</returns>
        public virtual async Task<EventGridEvent[]> DeserializeEventGridEventsAsync(string requestContent, CancellationToken cancellationToken = default)
            => await DeserializeEventGridEventsInternal(requestContent, true, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in the EventGrid event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in the EventGrid event schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of EventGrid Events.</returns>
        public virtual EventGridEvent[] DeserializeEventGridEvents(string requestContent, CancellationToken cancellationToken = default)
            => DeserializeEventGridEventsInternal(requestContent, false, cancellationToken).EnsureCompleted();

        internal Task<EventGridEvent[]> DeserializeEventGridEventsInternal(string requestContent, bool async, CancellationToken cancellationToken = default)
        {
            // need to check if events are actually encoded in the eg schema
            if (async)
            {

            }

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));

            // note: need parameterless constructor generated
            EventGridEvent[] egEvents = (EventGridEvent[])ObjectSerializer.Deserialize(stream, typeof(EventGridEvent[]), cancellationToken);

             foreach (EventGridEvent egEvent in egEvents)
            {
                if (SystemEventTypeMappingsTypes.SystemEventMappings.TryGetValue(egEvent.EventType, out Type typeOfEventData))
                {
                    //if (egEvent.Data != null)
                    //{
                    //    MemoryStream dataStream = new MemoryStream(Encoding.UTF8.GetBytes(egEvent.Data.ToString()));
                    //    if (async)
                    //    {
                    //        egEvent.Data = await ObjectSerializer.DeserializeAsync(dataStream, typeOfEventData, cancellationToken).ConfigureAwait(false);
                    //    }
                    //    else
                    //    {
                    //        egEvent.Data = ObjectSerializer.Deserialize(dataStream, typeOfEventData, cancellationToken);
                    //    }
                    //}
                }
                // First, let's attempt to find the mapping for the deserialization function in the system event type mapping.
                //if (SystemEventTypeMappings.SystemEventDeserializers.TryGetValue(egEvent.EventType, out Func<JsonElement, object> systemDeserializationFunction))
                //{
                //    if (egEvent.Data != null)
                //    {
                //        string eventDataContent = egEvent.Data.ToString();
                //        JsonDocument document;
                //        if (async)
                //        {
                //            document = await JsonDocument.ParseAsync(new MemoryStream(Encoding.UTF8.GetBytes(eventDataContent)), default, cancellationToken).ConfigureAwait(false);
                //        }
                //        else
                //        {
                //            document = JsonDocument.Parse(new MemoryStream(Encoding.UTF8.GetBytes(eventDataContent)), default);
                //        }

                //        egEvent.Data = systemDeserializationFunction(document.RootElement); // note: still need to generate setters for event grid event
                //    }
                //}
                // If not a system event, let's attempt to find the mapping for the event type in the custom event mapping.
                else if (TryGetCustomEventMapping(egEvent.EventType, out typeOfEventData))
                {
                    // doesn't work with primitive types/strings
                    //MemoryStream dataStream = new MemoryStream(Encoding.UTF8.GetBytes(egEvent.Data.ToString()));
                    //if (async)
                    //{
                    //    egEvent.Data = await ObjectSerializer.DeserializeAsync(dataStream, typeOfEventData, cancellationToken).ConfigureAwait(false);
                    //}
                    //else
                    //{
                    //    egEvent.Data = ObjectSerializer.Deserialize(dataStream, typeOfEventData, cancellationToken);
                    //}
                }
            }

            return Task.FromResult(egEvents);
        }

        ///// <summary>
        ///// Deserializes JSON encoded events and returns an array of events encoded in the CloudEvent schema.
        ///// </summary>
        ///// <param name="requestContent">
        ///// The JSON encoded representation of either a single event or an array or events, encoded in the CloudEvent schema.
        ///// </param>
        ///// <param name="cancellationToken"> The cancellation token to use. </param>
        ///// <returns>A list of CloudEvents.</returns>
        //public virtual async Task<CloudEvent[]> DeserializeCloudEventsAsync(string requestContent, CancellationToken cancellationToken = default)
        //    => await DeserializeCloudEventsInternal(requestContent, true, cancellationToken).ConfigureAwait(false);

        ///// <summary>
        ///// Deserializes JSON encoded events and returns an array of events encoded in the CloudEvent schema.
        ///// </summary>
        ///// <param name="requestContent">
        ///// The JSON encoded representation of either a single event or an array or events, encoded in the CloudEvent schema.
        ///// </param>
        ///// <param name="cancellationToken"> The cancellation token to use. </param>
        ///// <returns>A list of CloudEvents.</returns>
        //public virtual CloudEvent[] DeserializeCloudEvents(string requestContent, CancellationToken cancellationToken = default)
        //    => DeserializeCloudEventsInternal(requestContent, false, cancellationToken).EnsureCompleted();

        //internal async Task<CloudEvent[]> DeserializeCloudEventsInternal(string requestContent, bool async, CancellationToken cancellationToken = default)
        //{
        //    // need to check if events are actually encoded in the cloudevent schema

        //    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));

        //    // note: need parameterless constructor generated
        //    CloudEvent[] cloudEvents = (CloudEvent[])ObjectSerializer.Deserialize(stream, typeof(CloudEvent[]), cancellationToken);

        //    foreach (CloudEvent cloudEvent in cloudEvents)
        //    {

        //        // First, let's attempt to find the mapping for the deserialization function in the system event type mapping.
        //        if (SystemEventTypeMappings.SystemEventDeserializers.TryGetValue(cloudEvent.Type, out Func<JsonElement, object> systemDeserializationFunction))
        //        {
        //            if (cloudEvent.Data != null)
        //            {
        //                string eventDataContent = cloudEvent.Data.ToString();
        //                JsonDocument document;
        //                if (async)
        //                {
        //                    document = await JsonDocument.ParseAsync(new MemoryStream(Encoding.UTF8.GetBytes(eventDataContent)), default, cancellationToken).ConfigureAwait(false);
        //                }
        //                else
        //                {
        //                    document = JsonDocument.Parse(new MemoryStream(Encoding.UTF8.GetBytes(eventDataContent)), default);
        //                }

        //                cloudEvent.Data = systemDeserializationFunction(document.RootElement); // note: still need to generate setters for event grid event
        //            }
        //        }
        //        // If not a system event, let's attempt to find the mapping for the event type in the custom event mapping.
        //        else if (TryGetCustomEventMapping(cloudEvent.Type, out Type typeOfEventData))
        //        {
        //            // doesn't work with primitive types/strings
        //            MemoryStream dataStream = new MemoryStream(Encoding.UTF8.GetBytes(cloudEvent.Data.ToString()));
        //            if (async)
        //            {
        //                cloudEvent.Data = await ObjectSerializer.DeserializeAsync(dataStream, typeOfEventData, cancellationToken).ConfigureAwait(false);
        //            }
        //            else
        //            {
        //                cloudEvent.Data = ObjectSerializer.Deserialize(dataStream, typeOfEventData, cancellationToken);
        //            }
        //        }
        //    }

        //    return cloudEvents;
        //}

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in a custom event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in a custom event schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of custom events.</returns>
        public virtual async Task<T[]> DeserializeCustomEventsAsync<T>(string requestContent, CancellationToken cancellationToken = default)
            => await DeserializeCustomEventsInternal<T>(requestContent, true, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in a custom event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in a custom event schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of CloudEvents.</returns>
        public virtual T[] DeserializeCustomEvents<T>(string requestContent, CancellationToken cancellationToken = default)
            => DeserializeCustomEventsInternal<T>(requestContent, false, cancellationToken).EnsureCompleted();

        internal async Task<T[]> DeserializeCustomEventsInternal<T>(string requestContent, bool async, CancellationToken cancellationToken = default)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(requestContent));

            if (async)
            {
                return (T[])await ObjectSerializer.DeserializeAsync(stream, typeof(T[]), cancellationToken).ConfigureAwait(false);
            }
            else
            {
                return (T[])ObjectSerializer.Deserialize(stream, typeof(T[]), cancellationToken);
            }
        }

        /// <summary>
        /// Adds or updates a custom event mapping that associates an eventType string with the corresponding type of event data.
        /// </summary>
        /// <param name="eventType">The event type to register, such as "Contoso.Items.ItemReceived"</param>
        /// <param name="eventDataType">The type of eventdata corresponding to this eventType, such as typeof(ContosoItemReceivedEventData)</param>
        public void AddOrUpdateCustomEventMapping(string eventType, Type eventDataType)
        {
            ValidateEventType(eventType);

            if (eventDataType == null)
            {
                throw new ArgumentNullException(nameof(eventDataType));
            }

            _customEventTypeMappings.AddOrUpdate(
                eventType,
                eventDataType,
                (_, existingValue) => eventDataType);
        }

        /// <summary>
        /// Gets information about a custom event mapping.
        /// </summary>
        /// <param name="eventType">The registered event type, such as "Contoso.Items.ItemReceived"</param>
        /// <param name="eventDataType">The type of eventdata corresponding to this eventType, such as typeof(ContosoItemReceivedEventData)</param>
        /// <returns>True if the specified mapping exists.</returns>
        public bool TryGetCustomEventMapping(string eventType, out Type eventDataType)
        {
            ValidateEventType(eventType);

            return _customEventTypeMappings.TryGetValue(eventType, out eventDataType);
        }

        /// <summary>
        /// List all registered custom event mappings.
        /// </summary>
        /// <returns>An IEnumerable of mappings</returns>
        public IEnumerable<KeyValuePair<string, Type>> ListAllCustomEventMappings()
        {
            foreach (KeyValuePair<string, Type> kvp in _customEventTypeMappings)
            {
                yield return kvp;
            }
        }

        /// <summary>
        /// Removes a custom event mapping.
        /// </summary>
        /// <param name="eventType">The registered event type, such as "Contoso.Items.ItemReceived"</param>
        /// <param name="eventDataType">The type of eventdata corresponding to this eventType, such as typeof(ContosoItemReceivedEventData)</param>
        /// <returns>True if the specified mapping was removed successfully.</returns>
        public bool TryRemoveCustomEventMapping(string eventType, out Type eventDataType)
        {
            ValidateEventType(eventType);
            return _customEventTypeMappings.TryRemove(eventType, out eventDataType);
        }

        internal static void ValidateEventType(string eventType)
        {
            if (string.IsNullOrEmpty(eventType))
            {
                throw new ArgumentNullException(nameof(eventType));
            }
        }
    }
}
