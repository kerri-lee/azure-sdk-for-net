// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Messaging.EventGrid.Models;

namespace Azure.Messaging.EventGrid
{
    /// <summary>
    /// Class used to decode events from the Event Grid service.
    /// </summary>
    public class EventGridConsumer
    {
        private readonly ObjectSerializer _objectSerializer;
        private readonly ConcurrentDictionary<string, Type> _customEventTypeMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventGridConsumer"/> class.
        /// </summary>
        public EventGridConsumer()
        {
            // Initialize some sort of dictionary for custom event types
            _customEventTypeMappings = new ConcurrentDictionary<string, Type>();
            _objectSerializer = new JsonObjectSerializer(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in the EventGrid event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in the EventGrid schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of EventGrid Events.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<EventGridEvent[]> DeserializeEventGridEventsAsync(string requestContent, CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
            // use _objectSerializer to deserialize into list of eg events
            // need to check if events are actually encoded in the eg schema
        }

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in the CloudEvent schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in the CloudEvent schema.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of CloudEvents.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<CloudEvent[]> DeserializeCloudEventsAsync(string requestContent, CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
            // use _objectSerializer to deserialize into list of cloud events
            // need to check if events are actually encoded in the cloudevent schema
        }

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in a custom event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in a custom event schema.
        /// </param>
        /// <param name="customEventType">
        /// Custom event type that <paramref name="requestContent"/> will be decoded into.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of custom events.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<object[]> DeserializeCustomEventsAsync(string requestContent, Type customEventType, CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes JSON encoded events and returns an array of events encoded in a custom event schema.
        /// </summary>
        /// <param name="requestContent">
        /// The JSON encoded representation of either a single event or an array or events, encoded in a custom event schema.
        /// </param>
        /// <param name="customEventType">
        /// Custom event type that <paramref name="requestContent"/> will be decoded into.
        /// </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns>A list of CloudEvents.</returns>
        public virtual object[] DeserializeCustomEvents(string requestContent, Type customEventType, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            // use _objectSerializer to deserialize into list of custom events
            // need to check if events are actually encoded in the custom event schema
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

        internal void ValidateEventType(string eventType)
        {
            if (string.IsNullOrEmpty(eventType))
            {
                throw new ArgumentNullException(nameof(eventType));
            }
        }
    }
}
