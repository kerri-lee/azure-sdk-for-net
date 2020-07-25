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
            string resource = TestEnvironment.TopicHost + "?api-version=2018-01-01";
            string sasToken = EventGridPublisherClient.BuildSharedAccessSignature(resource, DateTimeOffset.UtcNow.AddMinutes(60), new AzureKeyCredential(TestEnvironment.TopicKey));

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
                        Id = Recording.Random.NewGuid().ToString()
                    });
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithCustomPayload()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                CloudEvent cloudEvent = new CloudEvent(
                    $"Subject-{i}",
                    "record");
                cloudEvent.Data = BinaryData.Serialize(new TestPayload("name", i));
                cloudEvent.Id = Recording.Random.NewGuid().ToString();
                eventsList.Add(cloudEvent);
            }

            return eventsList;
        }

        private IList<CloudEvent> GetCloudEventsListWithJsonString()
        {
            List<CloudEvent> eventsList = new List<CloudEvent>();

            for (int i = 0; i < 10; i++)
            {
                CloudEvent cloudEvent = new CloudEvent(
                    $"Subject-{i}",
                    "record");
                cloudEvent.Data = new BinaryData("[{   \"topic\": \"/subscriptions/id/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/xstoretestaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/testfile.txt\",  \"eventType\": \"Microsoft.Storage.BlobDeleted\",  \"eventTime\": \"2017-11-07T20:09:22.5674003Z\",  \"id\": \"4c2359fe-001e-00ba-0e04-58586806d298\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",   \"brandNewProperty\": \"0000000000000281000000000002F5CA\", \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}]");
                cloudEvent.Id = Recording.Random.NewGuid().ToString();
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
