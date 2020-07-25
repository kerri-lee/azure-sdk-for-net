// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Messaging.EventGrid.Models;

namespace Azure.Messaging.EventGrid
{
    /// <summary>
    /// Client used to interact with the Event Grid service.
    /// </summary>
    public class EventGridPublisherClient
    {
        private readonly ServiceRestClient _serviceRestClient;
        private readonly ClientDiagnostics _clientDiagnostics;
        private string _hostName => _endpoint.Host;
        private readonly Uri _endpoint;
        private readonly AzureKeyCredential _key;
        private string _apiVersion;

        /// <summary>Initalizes an instance of EventGridClient.</summary>
        protected EventGridPublisherClient()
        {
        }

        /// <summary>Initalizes an instance of EventGridClient.</summary>
        /// <param name="endpoint">Topic endpoint. For example, "https://TOPIC-NAME.REGION-NAME-1.eventgrid.azure.net/api/events".</param>
        /// <param name="credential">Credential used to connect to Azure.</param>
        public EventGridPublisherClient(Uri endpoint, AzureKeyCredential credential)
            : this(endpoint, credential, new EventGridClientOptions())
        {
        }

        /// <summary>Initalizes an instance of EventGridClient.</summary>
        /// <param name="endpoint">Topic endpoint. For example, "https://TOPIC-NAME.REGION-NAME-1.eventgrid.azure.net/api/events".</param>
        /// <param name="credential">Credential used to connect to Azure.</param>
        public EventGridPublisherClient(Uri endpoint, EventGridSharedAccessSignatureCredential credential)
            : this(endpoint, credential, new EventGridClientOptions())
        {
        }

        /// <summary>Initalizes an instance of the<see cref="EventGridPublisherClient"/> class.</summary>
        /// <param name="endpoint">Topic endpoint. For example, "https://TOPIC-NAME.REGION-NAME-1.eventgrid.azure.net/api/events".</param>
        /// <param name="credential">Credential used to connect to Azure.</param>
        /// <param name="options">Configuring options.</param>
        public EventGridPublisherClient(Uri endpoint, AzureKeyCredential credential, EventGridClientOptions options)
        {
            Argument.AssertNotNull(credential, nameof(credential));
            options ??= new EventGridClientOptions();
            _apiVersion = options.GetVersionString();
            _endpoint = endpoint;
            _key = credential;
            HttpPipeline pipeline = HttpPipelineBuilder.Build(options, new AzureKeyCredentialPolicy(credential, Constants.SasKeyName));
            _serviceRestClient = new ServiceRestClient(new ClientDiagnostics(options), pipeline, options.GetVersionString());
            _clientDiagnostics = new ClientDiagnostics(options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventGridPublisherClient"/> class.
        /// </summary>
        /// <param name="endpoint">Topic endpoint. For example, "https://TOPIC-NAME.REGION-NAME-1.eventgrid.azure.net/api/events".</param>
        /// <param name="credential">Credential used to connect to Azure.</param>
        /// <param name="options">Configuring options.</param>
        public EventGridPublisherClient(Uri endpoint, EventGridSharedAccessSignatureCredential credential, EventGridClientOptions options)
        {
            Argument.AssertNotNull(credential, nameof(credential));
            options ??= new EventGridClientOptions();
            _endpoint = endpoint;
            HttpPipeline pipeline = HttpPipelineBuilder.Build(options, new EventGridSharedAccessSignatureCredentialPolicy(credential));
            _serviceRestClient = new ServiceRestClient(new ClientDiagnostics(options), pipeline, options.GetVersionString());
            _clientDiagnostics = new ClientDiagnostics(options);
        }

        /// <summary> Publishes a batch of EventGridEvents to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> SendEventsAsync(IEnumerable<EventGridEvent> events, CancellationToken cancellationToken = default)
            => await SendEventsInternal(events, true /*async*/, cancellationToken).ConfigureAwait(false);

        /// <summary> Publishes a batch of EventGridEvents to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response SendEvents(IEnumerable<EventGridEvent> events, CancellationToken cancellationToken = default)
            => SendEventsInternal(events, false /*async*/, cancellationToken).EnsureCompleted();

        /// <summary> Publishes a batch of EventGridEvents to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="async">Whether to invoke the operation asynchronously.</param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        private async Task<Response> SendEventsInternal(IEnumerable<EventGridEvent> events, bool async, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(EventGridPublisherClient)}.{nameof(SendEvents)}");
            scope.Start();

            try
            {
                // List of events cannot be null
                Argument.AssertNotNull(events, nameof(events));

                List<EventGridEventInternal> eventsWithSerializedPayloads = new List<EventGridEventInternal>();
                foreach (EventGridEvent egEvent in events)
                {
                    // Events cannot be null
                    Argument.AssertNotNull(egEvent, nameof(egEvent));

                    EventGridEventInternal newEGEvent = new EventGridEventInternal(
                            egEvent.Id,
                            egEvent.Subject,
                            new EventGridSerializer(
                                egEvent.Data,
                                cancellationToken),
                            egEvent.EventType,
                            egEvent.EventTime,
                            egEvent.DataVersion);
                    newEGEvent.Topic = egEvent.Topic;

                    eventsWithSerializedPayloads.Add(newEGEvent);
                }
                if (async)
                {
                    // Publish asynchronously if called via an async path
                    return await _serviceRestClient.PublishEventsAsync(
                        _hostName,
                        eventsWithSerializedPayloads,
                        cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    return _serviceRestClient.PublishEvents(
                        _hostName,
                        eventsWithSerializedPayloads,
                        cancellationToken);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Publishes a batch of CloudEvents to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> SendEventsAsync(IEnumerable<CloudEvent> events, CancellationToken cancellationToken = default)
            => await SendCloudEventsInternal(events, true /*async*/, cancellationToken).ConfigureAwait(false);

        /// <summary> Publishes a batch of CloudEvents to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response SendEvents(IEnumerable<CloudEvent> events, CancellationToken cancellationToken = default)
            => SendCloudEventsInternal(events, false /*async*/, cancellationToken).EnsureCompleted();

        /// <summary> Publishes a batch of CloudEvents to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="async">Whether to invoke the operation asynchronously.</param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        private async Task<Response> SendCloudEventsInternal(IEnumerable<CloudEvent> events, bool async, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(EventGridPublisherClient)}.{nameof(SendEvents)}");
            scope.Start();

            try
            {
                // List of events cannot be null
                Argument.AssertNotNull(events, nameof(events));

                List<CloudEventInternal> eventsWithSerializedPayloads = new List<CloudEventInternal>();
                foreach (CloudEvent cloudEvent in events)
                {
                    // Events cannot be null
                    Argument.AssertNotNull(cloudEvent, nameof(cloudEvent));

                    CloudEventInternal newCloudEvent = new CloudEventInternal(
                        cloudEvent.Id,
                        cloudEvent.Source,
                        cloudEvent.Type,
                        cloudEvent.SpecVersion)
                    {
                        Time = cloudEvent.Time,
                        Datacontenttype = cloudEvent.DataContentType,
                        Dataschema = cloudEvent.DataSchema,
                        Subject = cloudEvent.Subject
                    };

                    // The 'Data' field is optional for the CloudEvent spec
                    if (!cloudEvent.Data.Equals(default(BinaryData)))
                    {
                        if (cloudEvent.Data.Format == BinaryDataFormat.Binary)
                        {
                            newCloudEvent.DataBase64 = Convert.ToBase64String(cloudEvent.Data.Bytes.ToArray());
                        }
                        else
                        {
                            newCloudEvent.Data = new EventGridSerializer(
                                cloudEvent.Data,
                                cancellationToken);
                        }
                    }
                    eventsWithSerializedPayloads.Add(newCloudEvent);
                }
                if (async)
                {
                    // Publish asynchronously if called via an async path
                    return await _serviceRestClient.PublishCloudEventEventsAsync(
                        _hostName,
                        eventsWithSerializedPayloads,
                        cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    return _serviceRestClient.PublishCloudEventEvents(
                        _hostName,
                        eventsWithSerializedPayloads,
                        cancellationToken);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Publishes a batch of custom events to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> SendEventsAsync(IEnumerable<object> events, CancellationToken cancellationToken = default)
            => await SendCustomEventsInternal(events, true /*async*/, cancellationToken).ConfigureAwait(false);

        /// <summary> Publishes a batch of custom events to an Azure Event Grid topic. </summary>
        /// <param name="events"> An array of events to be published to Event Grid. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response SendEvents(IEnumerable<object> events, CancellationToken cancellationToken = default)
            => SendCustomEventsInternal(events, false /*async*/, cancellationToken).EnsureCompleted();

        private async Task<Response> SendCustomEventsInternal(IEnumerable<object> events, bool async, CancellationToken cancellationToken = default)
        {
            using DiagnosticScope scope = _clientDiagnostics.CreateScope($"{nameof(EventGridPublisherClient)}.{nameof(SendEvents)}");
            scope.Start();

            try
            {
                // List of events cannot be null
                Argument.AssertNotNull(events, nameof(events));

                List<EventGridSerializer> serializedEvents = new List<EventGridSerializer>();
                foreach (object customEvent in events)
                {
                    // Events cannot be null
                    Argument.AssertNotNull(customEvent, nameof(customEvent));

                    serializedEvents.Add(
                        new EventGridSerializer(
                            BinaryData.Serialize(customEvent),
                            cancellationToken));
                }
                if (async)
                {
                    return await _serviceRestClient.PublishCustomEventEventsAsync(
                    _hostName,
                    serializedEvents,
                    cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    return _serviceRestClient.PublishCustomEventEvents(
                    _hostName,
                    serializedEvents,
                    cancellationToken);
                }
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Creates a SAS token for use with Event Grid service.
        /// </summary>
        /// <param name="resource">The path for the event grid topic to which you're sending events. For example, https://TOPIC-NAME.REGION-NAME.eventgrid.azure.net/eventGrid/api/events?api-version=2019-06-01. </param>
        /// <param name="expirationUtc">Time at which the SAS token becomes invalid for authentication.</param>
        /// <param name="key">Key credential used to generate the token.</param>
        /// <returns>Returns the generated SAS token string.</returns>
        public static string BuildSharedAccessSignature(string resource, DateTimeOffset expirationUtc, AzureKeyCredential key)
        {
            const char Resource = 'r';
            const char Expiration = 'e';
            const char Signature = 's';

            string encodedResource = HttpUtility.UrlEncode(resource);
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var encodedExpirationUtc = HttpUtility.UrlEncode(expirationUtc.ToString(culture));

            string unsignedSas = $"{Resource}={encodedResource}&{Expiration}={encodedExpirationUtc}";
            using (var hmac = new HMACSHA256(Convert.FromBase64String(key.Key)))
            {
                string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedSas)));
                string encodedSignature = HttpUtility.UrlEncode(signature);
                string signedSas = $"{unsignedSas}&{Signature}={encodedSignature}";

                return signedSas;
            }
        }
    }
}
