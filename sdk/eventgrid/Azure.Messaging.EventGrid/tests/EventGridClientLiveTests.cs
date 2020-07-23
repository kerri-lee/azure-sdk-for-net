// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.TestFramework;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.Models;
using NUnit.Framework;

namespace Azure.Messaging.EventGrid.Tests
{
    public class EventGridClientLiveTests : EventGridLiveTestBase
    {
        public EventGridClientLiveTests(bool async)
            : base(async)
        {
        }

        [Test]
        public async Task CanPublishEvent()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.TopicHost),
                    new AzureKeyCredential(TestEnvironment.TopicKey),
                    options));
            await client.SendEventsAsync(GetEventsList());
        }

        [Test]
        public async Task CanPublishEventWithCustomObjectPayload()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.TopicHost),
                    new AzureKeyCredential(TestEnvironment.TopicKey),
                    options));
            await client.SendEventsAsync(GetEventsListWithCustomPayload());
        }

        [Test]
        public async Task CanPublishEventToDomain()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.DomainHost),
                    new AzureKeyCredential(TestEnvironment.DomainKey),
                    options));
            await client.SendEventsAsync(GetEventsWithTopicsList());
        }

        [Test]
        public async Task CanPublishCloudEvent()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.SendEventsAsync(GetCloudEventsList());
        }

        [Test]
        public async Task CanPublishCloudEventWithCustomObjectPayload()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.SendEventsAsync(GetCloudEventsListWithCustomPayload());
        }

        [Test]
        public async Task CanPublishCustomEvent()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CustomEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CustomEventTopicKey),
                    options));
            await client.SendEventsAsync(GetCustomEventsList());
        }

        [Test]
        public async Task CanPublishEventUsingSAS()
        {
            EventGridPublisherClient client = new EventGridPublisherClient(
                new Uri(TestEnvironment.TopicHost),
                new AzureKeyCredential(TestEnvironment.TopicKey));

            string sasToken = client.BuildSharedAccessSignature(DateTimeOffset.UtcNow.AddMinutes(60));

            EventGridPublisherClient sasTokenClient = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.TopicHost),
                    new EventGridSharedAccessSignatureCredential(sasToken),
                    Recording.InstrumentClientOptions(new EventGridClientOptions())));
            await sasTokenClient.SendEventsAsync(GetEventsList());
        }

        //[Test]
        //public async Task CustomizeSerializedJSONPropertiesToCamelCase()
        //{
        //    EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
        //    options.Serializer = new JsonObjectSerializer(
        //        new JsonSerializerOptions()
        //        {
        //            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        //        });

        //    EventGridPublisherClient client = InstrumentClient(
        //        new EventGridPublisherClient(
        //            new Uri(TestEnvironment.CustomEventTopicHost),
        //            new AzureKeyCredential(TestEnvironment.CustomEventTopicKey),
        //            options));
        //    await client.SendAsync(GetCustomEventsList());
        //}

        private IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new EventGridEvent(
                        Recording.Random.NewGuid().ToString(),
                        $"Subject-{i}",
                        new BinaryData("hello"),
                        "Microsoft.MockPublisher.TestEvent",
                        Recording.Now,
                        "1.0"));
            }

            return eventsList;
        }

        private IList<EventGridEvent> GetEventsListWithCustomPayload()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new EventGridEvent(
                        Recording.Random.NewGuid().ToString(),
                        $"Subject-{i}",
                        BinaryData.Serialize(new TestPayload("name", i)),
                        "Microsoft.MockPublisher.TestEvent",
                        Recording.Now,
                        "1.0"));
            }

            return eventsList;
        }

        private IList<EventGridEvent> GetEventsWithTopicsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                EventGridEvent newEGEvent = new EventGridEvent(
                        Recording.Random.NewGuid().ToString(),
                        $"Subject-{i}",
                        new BinaryData("hello"),
                        "Microsoft.MockPublisher.TestEvent",
                        Recording.Now,
                        "1.0");
                newEGEvent.Topic = $"Topic-{i}";

                eventsList.Add(newEGEvent);
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsList()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new CloudEvent(
                        Recording.Random.NewGuid().ToString(),
                        $"Subject-{i}",
                        "record",
                        "1.0"));
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithCustomPayload()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                CloudEvent cloudEvent = new CloudEvent(
                    Recording.Random.NewGuid().ToString(),
                    $"Subject-{i}",
                    "record",
                    "1.0");
                cloudEvent.Data = BinaryData.Serialize(new TestPayload("name", i));
                eventsList.Add(cloudEvent);
            }

            return eventsList;
        }

        private IList<object> GetCustomEventsList()
        {
            List<object> eventsList = new List<object>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(new TestEvent()
                {
                    DataVersion = "1.0",
                    EventTime = Recording.Now,
                    EventType = "Microsoft.MockPublisher.TestEvent",
                    Id = Recording.Random.NewGuid().ToString(),
                    Subject = $"Subject-{i}",
                    Topic = $"Topic-{i}"
                });
            }

            return eventsList;
        }
    }
}
