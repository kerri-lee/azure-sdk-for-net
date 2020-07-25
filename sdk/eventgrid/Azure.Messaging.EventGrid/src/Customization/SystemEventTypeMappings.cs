// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Azure.Messaging.EventGrid.Models;

namespace Azure.Messaging.EventGrid
{
    internal class SystemEventTypeMappings
    {
        public static readonly IReadOnlyDictionary<string, Func<JsonElement, object>> SystemEventDeserializers = new Dictionary<string, Func<JsonElement, object>>(StringComparer.OrdinalIgnoreCase)
        {
            //// KEEP THIS SORTED BY THE NAME OF THE PUBLISHING SERVICE
            //// Add handling for additional event types here.
            //// NOTE: If any of the event data fields is polymorphic, remember to add an entry for the discriminator/BaseType
            //// in EventGridSubscriber.GetJsonSerializerWithPolymorphicSupport()
            //// Example: jsonSerializer.Converters.Add(new PolymorphicDeserializeJsonConverter<JobOutput>("@odata.type"));

            //// Event Hub events
            //{ EventTypes.EventHubCaptureFileCreatedEvent, EventHubCaptureFileCreatedEventData.DeserializeEventHubCaptureFileCreatedEventData },

            //// Resource Manager (Azure Subscription/Resource Group) events
            //{ EventTypes.ResourceWriteSuccessEvent, ResourceWriteSuccessData.DeserializeResourceWriteSuccessData },
            //{ EventTypes.ResourceWriteFailureEvent, ResourceWriteFailureData.DeserializeResourceWriteFailureData },
            //{ EventTypes.ResourceWriteCancelEvent, ResourceWriteCancelData.DeserializeResourceWriteCancelData },
            //{ EventTypes.ResourceDeleteSuccessEvent, ResourceDeleteSuccessData.DeserializeResourceDeleteSuccessData },
            //{ EventTypes.ResourceDeleteFailureEvent, ResourceDeleteFailureData.DeserializeResourceDeleteFailureData },
            //{ EventTypes.ResourceDeleteCancelEvent, ResourceDeleteCancelData.DeserializeResourceDeleteCancelData },
            //{ EventTypes.ResourceActionSuccessEvent, ResourceActionSuccessData.DeserializeResourceActionSuccessData },
            //{ EventTypes.ResourceActionFailureEvent, ResourceActionFailureData.DeserializeResourceActionFailureData },
            //{ EventTypes.ResourceActionCancelEvent, ResourceActionCancelData.DeserializeResourceActionCancelData },

            //// Storage events
            //{ EventTypes.StorageBlobCreatedEvent, StorageBlobCreatedEventData.DeserializeStorageBlobCreatedEventData },
            //{ EventTypes.StorageBlobDeletedEvent, StorageBlobDeletedEventData.DeserializeStorageBlobDeletedEventData },
            //{ EventTypes.StorageBlobRenamedEvent, StorageBlobRenamedEventData.DeserializeStorageBlobRenamedEventData },
            //{ EventTypes.StorageDirectoryCreatedEvent, StorageDirectoryCreatedEventData.DeserializeStorageDirectoryCreatedEventData },
            //{ EventTypes.StorageDirectoryDeletedEvent, StorageDirectoryDeletedEventData.DeserializeStorageDirectoryDeletedEventData },
            //{ EventTypes.StorageDirectoryRenamedEvent, StorageDirectoryRenamedEventData.DeserializeStorageDirectoryRenamedEventData }
        };
    }
}
