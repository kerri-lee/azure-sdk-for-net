// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.Messaging.EventGrid.Models
{
    public partial class StorageDirectoryCreatedEventData
    {
        internal static StorageDirectoryCreatedEventData DeserializeStorageDirectoryCreatedEventData(JsonElement element)
        {
            Optional<string> api = default;
            Optional<string> clientRequestId = default;
            Optional<string> requestId = default;
            Optional<string> eTag = default;
            Optional<string> url = default;
            Optional<string> sequencer = default;
            Optional<string> identity = default;
            Optional<object> storageDiagnostics = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("api"))
                {
                    api = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("clientRequestId"))
                {
                    clientRequestId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("requestId"))
                {
                    requestId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("eTag"))
                {
                    eTag = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("url"))
                {
                    url = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("sequencer"))
                {
                    sequencer = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("identity"))
                {
                    identity = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("storageDiagnostics"))
                {
                    storageDiagnostics = property.Value.GetObject();
                    continue;
                }
            }
            return new StorageDirectoryCreatedEventData(api.Value, clientRequestId.Value, requestId.Value, eTag.Value, url.Value, sequencer.Value, identity.Value, storageDiagnostics.Value);
        }
    }
}
