// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Azure.Core;
using Azure.Messaging.EventGrid.Models;
using Microsoft.Azure.EventGrid.Tests;
using NUnit.Framework;

namespace Azure.Messaging.EventGrid.Tests
{
    public class ConsumeEventTests
    {
        [Test]
        public void ConsumeStorageBlobDeletedEventWithExtraProperty()
        {
            string requestContent = "[{   \"topic\": \"/subscriptions/id/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/xstoretestaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/testfile.txt\",  \"eventType\": \"Microsoft.Storage.BlobDeleted\",  \"eventTime\": \"2017-11-07T20:09:22.5674003Z\",  \"id\": \"4c2359fe-001e-00ba-0e04-58586806d298\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",   \"brandNewProperty\": \"0000000000000281000000000002F5CA\", \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}]";

            EventGridEvent[] events = EventGridConsumer.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);

            var egEvent = events[0];

            if (egEvent.DataType == typeof(StorageBlobDeletedEventData))
            {
                StorageBlobDeletedEventData data = egEvent.Data.Deserialize<StorageBlobDeletedEventData>(new JsonObjectSerializer(new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
        }

        [Test]
        public void ConsumeCustomEvents()
        {
            string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": {    \"itemSku\": \"512d38b6-c7b8-40c8-89fe-f46f9e9622b6\",    \"itemUri\": \"https://rp-eastus2.eventgrid.azure.net:553/eventsubscriptions/estest/validate?id=B2E34264-7D71-453A-B5FB-B62D0FDC85EE&t=2018-04-26T20:30:54.4538837Z&apiVersion=2018-05-01-preview&token=1BNqCxBBSSE9OnNSfZM4%2b5H9zDegKMY6uJ%2fO2DFRkwQ%3d\"  },  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

            EventGridEvent[] events = EventGridConsumer.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            if (events[0].EventType == "Contoso.Items.ItemReceived")
            {
                ContosoItemReceivedEventData eventData = events[0].Data.Deserialize<ContosoItemReceivedEventData>(new JsonObjectSerializer(new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
                Assert.AreEqual("512d38b6-c7b8-40c8-89fe-f46f9e9622b6", eventData.ItemSku);
            }
        }

        [Test]
        public void ConsumeCustomEventWithArrayData()
        {
            string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": [{    \"itemSku\": \"512d38b6-c7b8-40c8-89fe-f46f9e9622b6\",    \"itemUri\": \"https://rp-eastus2.eventgrid.azure.net:553\"  }],  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

            EventGridEvent[] events = EventGridConsumer.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            if (events[0].EventType == "Contoso.Items.ItemReceived")
            {
                ContosoItemReceivedEventData[] eventData = events[0].Data.Deserialize<ContosoItemReceivedEventData[]>(new JsonObjectSerializer(new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
                Assert.AreEqual("512d38b6-c7b8-40c8-89fe-f46f9e9622b6", eventData[0].ItemSku);
            }
        }

        [Test]
        public void ConsumeCustomEventWithBooleanData()
        {
            string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": true,  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

            EventGridEvent[] events = EventGridConsumer.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            if (events[0].EventType == "Contoso.Items.ItemReceived")
            {
                bool eventData = events[0].Data.Deserialize<bool>();
                Assert.True(eventData);
            }
        }

        [Test]
        public void ConsumeCustomEventWithStringData()
        {
            string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": \"stringdata\",  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

            EventGridEvent[] events = EventGridConsumer.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            if (events[0].EventType == "Contoso.Items.ItemReceived")
            {
                string eventData = events[0].Data.Deserialize<string>();
                Assert.AreEqual("stringdata", eventData);
            }
        }

        [Test]
        public void ConsumeMultipleEventsInSameBatch()
        {
            string requestContent = "[ " +
                "{  \"topic\": \"/subscriptions/319a9601-1ec0-0000-aebc-8fe82724c81e/resourceGroups/testrg/providers/Microsoft.Storage/storageAccounts/myaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/file1.txt\",  \"eventType\": \"Microsoft.Storage.BlobCreated\",  \"eventTime\": \"2017-08-16T01:57:26.005121Z\",  \"id\": \"602a88ef-0001-00e6-1233-1646070610ea\",  \"data\": {    \"api\": \"PutBlockList\",    \"clientRequestId\": \"799304a4-bbc5-45b6-9849-ec2c66be800a\",    \"requestId\": \"602a88ef-0001-00e6-1233-164607000000\",    \"eTag\": \"0x8D4E44A24ABE7F1\",    \"contentType\": \"text/plain\",    \"contentLength\": 447,    \"blobType\": \"BlockBlob\",    \"url\": \"https://myaccount.blob.core.windows.net/testcontainer/file1.txt\",    \"sequencer\": \"00000000000000EB000000000000C65A\"  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}, " +
                "{   \"topic\": \"/subscriptions/id/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/xstoretestaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/testfile.txt\",  \"eventType\": \"Microsoft.Storage.BlobDeleted\",  \"eventTime\": \"2017-11-07T20:09:22.5674003Z\",  \"id\": \"4c2359fe-001e-00ba-0e04-58586806d298\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",    \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}, " +
                "{   \"topic\": \"/subscriptions/id/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/xstoretestaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/testfile.txt\",  \"eventType\": \"Microsoft.Storage.BlobDeleted\",  \"eventTime\": \"2017-11-07T20:09:22.5674003Z\",  \"id\": \"4c2359fe-001e-00ba-0e04-58586806d298\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",    \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}]";

            var events = EventGridConsumer.DeserializeEventGridEvents(requestContent);
            var objectSerializer = new JsonObjectSerializer(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            Assert.NotNull(events);
            Assert.AreEqual(3, events.Length);

            for (int i = 0; i < 3; i++)
            {
                if (events[i].DataType == typeof(StorageBlobCreatedEventData))
                {
                    Assert.IsTrue(i == 0);
                    StorageBlobCreatedEventData eventData = events[i].Data.Deserialize<StorageBlobCreatedEventData>(objectSerializer);
                }
                else if (events[i].DataType == typeof(StorageBlobDeletedEventData))
                {
                    Assert.IsTrue(i == 1 || i == 2);
                    StorageBlobDeletedEventData eventData = events[i].Data.Deserialize<StorageBlobDeletedEventData>(objectSerializer);
                    if (i == 2)
                    {
                        Assert.AreEqual("https://example.blob.core.windows.net/testcontainer/testfile.txt", eventData.Url);
                    }
                }
            }
        }

        [Test]
        public void ConsumeMultipleCustomEventsInSameBatch()
        {
            // note: properties needed to be camel cased
            string requestContent = "[" +
                "{\"dataVersion\":\"1.0\",\"eventTime\":\"2020-07-09T15:10:21.7694198-07:00\",\"eventType\":\"Microsoft.MockPublisher.TestEvent\",\"id\":\"2691836e-bf44-b259-9a54-9cc8cc984d37\",\"subject\":\"Subject-0\",\"topic\":\"Topic-0\"}," +
                "{\"dataVersion\":\"1.0\",\"eventTime\":\"2020-07-09T15:10:21.7694198-07:00\",\"eventType\":\"Microsoft.MockPublisher.TestEvent\",\"id\":\"2d99bf2a-5616-326c-13a5-00335045bc1b\",\"subject\":\"Subject-1\",\"topic\":\"Topic-1\"}," +
                "{\"dataVersion\":\"1.0\",\"eventTime\":\"2020-07-09T15:10:21.7694198-07:00\",\"eventType\":\"Microsoft.MockPublisher.TestEvent\",\"id\":\"7953a676-fece-944b-37ec-a8f03a806faf\",\"subject\":\"Subject-2\",\"topic\":\"Topic-2\"}," +
                "{\"dataVersion\":\"1.0\",\"eventTime\":\"2020-07-09T15:10:21.7694198-07:00\",\"eventType\":\"Microsoft.MockPublisher.TestEvent\",\"id\":\"2090f9a3-76c7-178f-165f-139fbe256155\",\"subject\":\"Subject-3\",\"topic\":\"Topic-3\"}," +
                "{\"dataVersion\":\"1.0\",\"eventTime\":\"2020-07-09T15:10:21.7694198-07:00\",\"eventType\":\"Microsoft.MockPublisher.TestEvent\",\"id\":\"25518097-d444-2c68-7404-f629ae7bf893\",\"subject\":\"Subject-4\",\"topic\":\"Topic-4\"}]";

            var events = EventGridConsumer.DeserializeCustomEvents<TestEvent>(requestContent);
            Assert.NotNull(events);
            Assert.AreEqual(5, events.Length);
            Assert.True(events[0] is TestEvent);
            Assert.True(events[0].EventType is "Microsoft.MockPublisher.TestEvent");
        }

        [Test]
        public void ConsumeStorageBlobDeletedCloudEventWithAdditionalProperties()
        {
            string requestContent = "[{ \"additionalProperties\":{ \"key\": \"value\" },  \"id\":\"994bc3f8-c90c-6fc3-9e83-6783db2221d5\",\"source\":\"Subject-0\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",   \"brandNewProperty\": \"0000000000000281000000000002F5CA\", \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  }, \"type\":\"Microsoft.Storage.BlobDeleted\",\"specversion\":\"1.0\"}]";

            CloudEvent[] events = EventGridConsumer.DeserializeCloudEvents(requestContent);

            Assert.NotNull(events);

            var cloudEvent = events[0];

            if (cloudEvent.DataType == typeof(StorageBlobDeletedEventData))
            {
                var data = cloudEvent.Data.Deserialize<StorageBlobDeletedEventData>(new JsonObjectSerializer(new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
        }

        [Test]
        public void ConsumeMultipleCloudEventsInSameBatch()
        {
            string requestContent = "[" +
                "{\"id\":\"994bc3f8-c90c-6fc3-9e83-6783db2221d5\",\"source\":\"Subject-0\",\"data\": {    \"api\": \"PutBlockList\",    \"clientRequestId\": \"799304a4-bbc5-45b6-9849-ec2c66be800a\",    \"requestId\": \"602a88ef-0001-00e6-1233-164607000000\",    \"eTag\": \"0x8D4E44A24ABE7F1\",    \"contentType\": \"text/plain\",    \"contentLength\": 447,    \"blobType\": \"BlockBlob\",    \"url\": \"https://myaccount.blob.core.windows.net/testcontainer/file1.txt\",    \"sequencer\": \"00000000000000EB000000000000C65A\"  },\"type\":\"record\",\"specversion\":\"1.0\"}," +
                "{\"id\":\"2947780a-356b-c5a5-feb4-f5261fb2f155\",\"source\":\"Subject-1\",\"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",    \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },\"type\":\"record\",\"specversion\":\"1.0\"}," +
                "{\"id\":\"cb14e05b-50c6-67dc-cafa-f4bcff3bf520\",\"source\":\"Subject-2\",\"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",    \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },\"type\":\"record\",\"specversion\":\"1.0\"}]";

            var events = EventGridConsumer.DeserializeCloudEvents(requestContent);
            var objectSerializer = new JsonObjectSerializer(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            Assert.NotNull(events);
            Assert.AreEqual(3, events.Length);

            for (int i = 0; i < 3; i++)
            {
                if (events[i].DataType == typeof(StorageBlobCreatedEventData))
                {
                    Assert.IsTrue(i == 0);
                    StorageBlobCreatedEventData eventData = events[i].Data.Deserialize<StorageBlobCreatedEventData>(objectSerializer);
                }
                else if (events[i].DataType == typeof(StorageBlobDeletedEventData))
                {
                    Assert.IsTrue(i == 1 || i == 2);
                    StorageBlobDeletedEventData eventData = events[i].Data.Deserialize<StorageBlobDeletedEventData>(objectSerializer);
                    if (i == 2)
                    {
                        Assert.AreEqual("https://example.blob.core.windows.net/testcontainer/testfile.txt", eventData.Url);
                    }
                }
            }
        }
    }
}
