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
        public readonly EventGridConsumer _eventGridConsumer;

        public ConsumeEventTests()
        {
            _eventGridConsumer = new EventGridConsumer();
        }

        [Test]
        public void ConsumeStorageBlobDeletedEventWithExtraProperty()
        {
            string requestContent = "[{   \"topic\": \"/subscriptions/id/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/xstoretestaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/testfile.txt\",  \"eventType\": \"Microsoft.Storage.BlobDeleted\",  \"eventTime\": \"2017-11-07T20:09:22.5674003Z\",  \"id\": \"4c2359fe-001e-00ba-0e04-58586806d298\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",   \"brandNewProperty\": \"0000000000000281000000000002F5CA\", \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}]";

            EventGridEvent[] events = _eventGridConsumer.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            Assert.True(events[0].Data is StorageBlobDeletedEventData);
            StorageBlobDeletedEventData eventData = (StorageBlobDeletedEventData)events[0].Data;
            Assert.AreEqual("https://example.blob.core.windows.net/testcontainer/testfile.txt", eventData.Url);
        }

        [Test]
        public void ConsumeCustomEvents()
        {
            string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": {    \"itemSku\": \"512d38b6-c7b8-40c8-89fe-f46f9e9622b6\",    \"itemUri\": \"https://rp-eastus2.eventgrid.azure.net:553/eventsubscriptions/estest/validate?id=B2E34264-7D71-453A-B5FB-B62D0FDC85EE&t=2018-04-26T20:30:54.4538837Z&apiVersion=2018-05-01-preview&token=1BNqCxBBSSE9OnNSfZM4%2b5H9zDegKMY6uJ%2fO2DFRkwQ%3d\"  },  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

            EventGridConsumer eventGridConsumer2 = new EventGridConsumer();

            // Testing update
            eventGridConsumer2.AddOrUpdateCustomEventMapping("Contoso.Items.ItemReceived", typeof(ContosoItemSentEventData));
            eventGridConsumer2.AddOrUpdateCustomEventMapping("Contoso.Items.ItemReceived", typeof(ContosoItemReceivedEventData));

            EventGridEvent[] events = eventGridConsumer2.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            Assert.True(events[0].Data is ContosoItemReceivedEventData);
            ContosoItemReceivedEventData eventData = (ContosoItemReceivedEventData)events[0].Data;
            Assert.AreEqual("512d38b6-c7b8-40c8-89fe-f46f9e9622b6", eventData.ItemSku);
        }

        [Test]
        public void ConsumeCustomEventWithArrayData()
        {
            string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": [{    \"itemSku\": \"512d38b6-c7b8-40c8-89fe-f46f9e9622b6\",    \"itemUri\": \"https://rp-eastus2.eventgrid.azure.net:553\"  }],  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

            EventGridConsumer eventGridConsumer2 = new EventGridConsumer();
            eventGridConsumer2.AddOrUpdateCustomEventMapping("Contoso.Items.ItemReceived", typeof(ContosoItemReceivedEventData[]));

            EventGridEvent[] events = eventGridConsumer2.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            Assert.True(events[0].Data is ContosoItemReceivedEventData[]);
            ContosoItemReceivedEventData[] eventData = (ContosoItemReceivedEventData[])events[0].Data;
            Assert.AreEqual("512d38b6-c7b8-40c8-89fe-f46f9e9622b6", eventData[0].ItemSku);
        }

        //[Test]
        //public void ConsumeCustomEventWithBooleanData()
        //{
        //    string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": true,  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

        //    EventGridConsumer eventGridConsumer2 = new EventGridConsumer();
        //    eventGridConsumer2.AddOrUpdateCustomEventMapping("Contoso.Items.ItemReceived", typeof(bool));

        //    EventGridEvent[] events = eventGridConsumer2.DeserializeEventGridEvents(requestContent);

        //    Assert.NotNull(events);
        //    Assert.True(events[0].Data is bool);
        //    bool eventData = (bool)events[0].Data;
        //    Assert.True(eventData);
        //}

        //[Test]
        //public void ConsumeCustomEventWithStringData()
        //{
        //    string requestContent = "[{  \"id\": \"2d1781af-3a4c-4d7c-bd0c-e34b19da4e66\",  \"topic\": \"/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\",  \"subject\": \"\",  \"data\": \"stringdata\",  \"eventType\": \"Contoso.Items.ItemReceived\",  \"eventTime\": \"2018-01-25T22:12:19.4556811Z\",  \"metadataVersion\": \"1\",  \"dataVersion\": \"1\"}]";

        //    JsonSerializerOptions options = new JsonSerializerOptions()
        //    {
        //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        //    };
        //    options.Converters.Add(new Converter());
        //    EventGridConsumer eventGridConsumer2 = new EventGridConsumer()
        //    {
        //        ObjectSerializer = new JsonObjectSerializer(options)
        //    };
        //    eventGridConsumer2.AddOrUpdateCustomEventMapping("Contoso.Items.ItemReceived", typeof(string));

        //    EventGridEvent[] events = eventGridConsumer2.DeserializeEventGridEvents(requestContent);

        //    Assert.NotNull(events);
        //    Assert.True(events[0].Data is string);
        //    string eventData = (string)events[0].Data;
        //    Assert.AreEqual("stringdata", eventData);
        //}

        [Test]
        public void TestCustomEventMappings()
        {
            EventGridConsumer eventGridConsumer2 = new EventGridConsumer();
            eventGridConsumer2.AddOrUpdateCustomEventMapping("Contoso.Items.ItemSent", typeof(ContosoItemSentEventData));
            eventGridConsumer2.AddOrUpdateCustomEventMapping("Contoso.Items.ItemReceived", typeof(ContosoItemReceivedEventData));

            IReadOnlyList<KeyValuePair<string, Type>> list = eventGridConsumer2.ListAllCustomEventMappings().ToList();
            Assert.AreEqual(2, list.Count);

            Assert.True(eventGridConsumer2.TryGetCustomEventMapping("Contoso.Items.ItemSent", out Type retrievedType));
            Assert.AreEqual(typeof(ContosoItemSentEventData), retrievedType);

            Assert.True(eventGridConsumer2.TryRemoveCustomEventMapping("Contoso.Items.ItemReceived", out retrievedType));
            Assert.AreEqual(typeof(ContosoItemReceivedEventData), retrievedType);
        }

        [Test]
        public void ConsumeMultipleEventsInSameBatch()
        {
            string requestContent = "[ " +
                "{  \"topic\": \"/subscriptions/319a9601-1ec0-0000-aebc-8fe82724c81e/resourceGroups/testrg/providers/Microsoft.Storage/storageAccounts/myaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/file1.txt\",  \"eventType\": \"Microsoft.Storage.BlobCreated\",  \"eventTime\": \"2017-08-16T01:57:26.005121Z\",  \"id\": \"602a88ef-0001-00e6-1233-1646070610ea\",  \"data\": {    \"api\": \"PutBlockList\",    \"clientRequestId\": \"799304a4-bbc5-45b6-9849-ec2c66be800a\",    \"requestId\": \"602a88ef-0001-00e6-1233-164607000000\",    \"eTag\": \"0x8D4E44A24ABE7F1\",    \"contentType\": \"text/plain\",    \"contentLength\": 447,    \"blobType\": \"BlockBlob\",    \"url\": \"https://myaccount.blob.core.windows.net/testcontainer/file1.txt\",    \"sequencer\": \"00000000000000EB000000000000C65A\"  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}, " +
                "{   \"topic\": \"/subscriptions/id/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/xstoretestaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/testfile.txt\",  \"eventType\": \"Microsoft.Storage.BlobDeleted\",  \"eventTime\": \"2017-11-07T20:09:22.5674003Z\",  \"id\": \"4c2359fe-001e-00ba-0e04-58586806d298\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",    \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}, " +
                "{   \"topic\": \"/subscriptions/id/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/xstoretestaccount\",  \"subject\": \"/blobServices/default/containers/testcontainer/blobs/testfile.txt\",  \"eventType\": \"Microsoft.Storage.BlobDeleted\",  \"eventTime\": \"2017-11-07T20:09:22.5674003Z\",  \"id\": \"4c2359fe-001e-00ba-0e04-58586806d298\",  \"data\": {    \"api\": \"DeleteBlob\",    \"requestId\": \"4c2359fe-001e-00ba-0e04-585868000000\",    \"contentType\": \"text/plain\",    \"blobType\": \"BlockBlob\",    \"url\": \"https://example.blob.core.windows.net/testcontainer/testfile.txt\",    \"sequencer\": \"0000000000000281000000000002F5CA\",    \"storageDiagnostics\": {      \"batchId\": \"b68529f3-68cd-4744-baa4-3c0498ec19f0\"    }  },  \"dataVersion\": \"\",  \"metadataVersion\": \"1\"}]";

            var events = _eventGridConsumer.DeserializeEventGridEvents(requestContent);

            Assert.NotNull(events);
            Assert.AreEqual(3, events.Length);
            Assert.True(events[0].Data is StorageBlobCreatedEventData);
            Assert.True(events[1].Data is StorageBlobDeletedEventData);
            Assert.True(events[2].Data is StorageBlobDeletedEventData);
            StorageBlobDeletedEventData eventData = (StorageBlobDeletedEventData)events[2].Data;
            Assert.AreEqual("https://example.blob.core.windows.net/testcontainer/testfile.txt", eventData.Url);
        }
    }
}
