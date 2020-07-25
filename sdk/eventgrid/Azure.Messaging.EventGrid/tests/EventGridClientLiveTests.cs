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
        public async Task CanPublishCloudEventWithRawJsonString()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.SendEventsAsync(GetCloudEventsListWithJsonString());
        }

        [Test]
        public async Task CanPublishCloudEventWithBinaryData()
        {
            EventGridClientOptions options = Recording.InstrumentClientOptions(new EventGridClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.SendEventsAsync(GetCloudEventsListWithBinaryData());
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
            string resource = TestEnvironment.TopicHost + "?api-version=2018-01-01";
            string sasToken = EventGridPublisherClient.BuildSharedAccessSignature(resource, DateTimeOffset.UtcNow.AddMinutes(60), new AzureKeyCredential(TestEnvironment.TopicKey));

            EventGridPublisherClient sasTokenClient = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.TopicHost),
                    new EventGridSharedAccessSignatureCredential(sasToken),
                    Recording.InstrumentClientOptions(new EventGridClientOptions())));
            await sasTokenClient.SendEventsAsync(GetEventsList());
        }

        private IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new EventGridEvent(
                        $"Subject-{i}",
                        new BinaryData("hello"),
                        "Microsoft.MockPublisher.TestEvent",
                        "1.0")
                    {
                        Id = Recording.Random.NewGuid().ToString(),
                        EventTime = Recording.Now
                    });
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
                        $"Subject-{i}",
                        BinaryData.Serialize(new TestPayload("name", i)),
                        "Microsoft.MockPublisher.TestEvent",
                        "1.0")
                    {
                        Id = Recording.Random.NewGuid().ToString(),
                        EventTime = Recording.Now
                    });
            }

            return eventsList;
        }

        private IList<EventGridEvent> GetEventsWithTopicsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                EventGridEvent newEGEvent = new EventGridEvent(
                        $"Subject-{i}",
                        new BinaryData("hello"),
                        "Microsoft.MockPublisher.TestEvent",
                        "1.0");
                newEGEvent.Id = Recording.Random.NewGuid().ToString();
                newEGEvent.EventTime = Recording.Now;
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
                        $"Subject-{i}",
                        "record")
                    {
                        Id = Recording.Random.NewGuid().ToString(),
                        Time = Recording.Now
                    });
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithCustomPayload()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new CloudEvent(
                        $"Subject-{i}",
                        "record")
                    {
                        Data = BinaryData.Serialize(new TestPayload("name", i)),
                        Id = Recording.Random.NewGuid().ToString(),
                        Time = Recording.Now
                    });
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithJsonString()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new CloudEvent(
                        $"Subject-{i}",
                        "record")
                    {
                        Data = new BinaryData("[{   \"property1\": \"abc\",  \"property2\": \"123\"}]"),
                        Id = Recording.Random.NewGuid().ToString(),
                        Time = Recording.Now
                    });
            }
            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithBinaryData()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new CloudEvent(
                        $"Subject-{i}",
                        "record")
                    {
                        Data = new BinaryData(Encoding.UTF8.GetBytes("data")),
                        Id = Recording.Random.NewGuid().ToString(),
                        Time = Recording.Now
                    });
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
