﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.TopicHost),
                    new AzureKeyCredential(TestEnvironment.TopicKey),
                    options));
            await client.PublishEventsAsync(GetEventsList());
        }

        [Test]
        public async Task CanPublishEventWithCustomObjectPayload()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.TopicHost),
                    new AzureKeyCredential(TestEnvironment.TopicKey),
                    options));
            await client.PublishEventsAsync(GetEventsListWithCustomPayload());
        }

        [Test]
        public async Task CanPublishEventToDomain()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.DomainHost),
                    new AzureKeyCredential(TestEnvironment.DomainKey),
                    options));
            await client.PublishEventsAsync(GetEventsWithTopicsList());
        }

        [Test]
        public async Task CanPublishCloudEvent()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.PublishCloudEventsAsync(GetCloudEventsList());
        }

        [Test]
        public async Task CanPublishCloudEventWithBinaryData()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.PublishCloudEventsAsync(GetCloudEventsListWithBinaryData());
        }

        [Test]
        public async Task CanPublishCloudEventWithRawJsonData()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.PublishCloudEventsAsync(GetCloudEventsListWithRawJsonData());
        }

        [Test]
        public async Task CanPublishCloudEventWithCustomObjectPayload()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CloudEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CloudEventTopicKey),
                    options));
            await client.PublishCloudEventsAsync(GetCloudEventsListWithCustomPayload());
        }

        [Test]
        public async Task CanPublishCustomEvent()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CustomEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CustomEventTopicKey),
                    options));
            await client.PublishCustomEventsAsync(GetCustomEventsList());
        }

        [Test]
        public async Task CanPublishEventUsingSAS()
        {
            string resource = TestEnvironment.TopicHost + "?api-version=2018-01-01";
            string sasToken = EventGridPublisherClient.BuildSharedAccessSignature(
                resource,
                DateTimeOffset.UtcNow.AddMinutes(60),
                new AzureKeyCredential(TestEnvironment.TopicKey));

            EventGridPublisherClient sasTokenClient = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.TopicHost),
                    new EventGridSharedAccessSignatureCredential(sasToken),
                    Recording.InstrumentClientOptions(new EventGridPublisherClientOptions())));
            await sasTokenClient.PublishEventsAsync(GetEventsList());
        }

        [Test]
        public async Task CustomizeSerializedJSONPropertiesToCamelCase()
        {
            EventGridPublisherClientOptions options = Recording.InstrumentClientOptions(new EventGridPublisherClientOptions());
            options.Serializer = new JsonObjectSerializer(
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            EventGridPublisherClient client = InstrumentClient(
                new EventGridPublisherClient(
                    new Uri(TestEnvironment.CustomEventTopicHost),
                    new AzureKeyCredential(TestEnvironment.CustomEventTopicKey),
                    options));
            await client.PublishCustomEventsAsync(GetCustomEventsList());
        }

        private IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(
                    new EventGridEvent()
                    {
                        Id = Recording.Random.NewGuid().ToString(),
                        Subject = $"Subject-{i}",
                        Data = "hello",
                        EventType = "Microsoft.MockPublisher.TestEvent",
                        EventTime = Recording.Now,
                        DataVersion = "1.0"
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
                    new EventGridEvent()
                    {
                        Id = Recording.Random.NewGuid().ToString(),
                        Subject = $"Subject-{i}",
                        Data = new TestPayload("name", i),
                        EventType = "Microsoft.MockPublisher.TestEvent",
                        EventTime = Recording.Now,
                        DataVersion = "1.0"
                    });
            }

            return eventsList;
        }

        private IList<EventGridEvent> GetEventsWithTopicsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                EventGridEvent newEGEvent = new EventGridEvent()
                {
                    Id = Recording.Random.NewGuid().ToString(),
                    Subject = $"Subject-{i}",
                    Data = "hello",
                    EventType = "Microsoft.MockPublisher.TestEvent",
                    EventTime = Recording.Now,
                    DataVersion = "1.0"
                };
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
                    new CloudEvent()
                    {
                        Id = Recording.Random.NewGuid().ToString(),
                        Subject = $"Subject-{i}",
                        Source = "record",
                        Time = Recording.Now,
                        Type = "Microsoft.MockPublisher.TestEvent"
                    });
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithCustomPayload()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                CloudEvent cloudEvent = new CloudEvent()
                {
                    Id = Recording.Random.NewGuid().ToString(),
                    Subject = $"Subject-{i}",
                    Source = "record",
                    Time = Recording.Now,
                    Type = "Microsoft.MockPublisher.TestEvent"
                };
            cloudEvent.Data = new TestPayload("name", i);
                eventsList.Add(cloudEvent);
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithBinaryData()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                CloudEvent cloudEvent = new CloudEvent()
                {
                    Id = Recording.Random.NewGuid().ToString(),
                    Subject = $"Subject-{i}",
                    Source = "record",
                    Time = Recording.Now,
                    Type = "Microsoft.MockPublisher.TestEvent"
                };
                cloudEvent.Data = Encoding.UTF8.GetBytes("data");
                eventsList.Add(cloudEvent);
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithRawJsonData()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                CloudEvent cloudEvent = new CloudEvent()
                {
                    Id = Recording.Random.NewGuid().ToString(),
                    Subject = $"Subject-{i}",
                    Source = "record",
                    Time = Recording.Now,
                    Type = "Microsoft.MockPublisher.TestEvent"
                };
                cloudEvent.Data = "{\"property1\": \"abc\",  \"property2\": \"123\"}";
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
