// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.Messaging.EventGrid.Models
{
    public partial class ResourceDeleteFailureData
    {
        internal static ResourceDeleteFailureData DeserializeResourceDeleteFailureData(JsonElement element)
        {
            Optional<string> tenantId = default;
            Optional<string> subscriptionId = default;
            Optional<string> resourceGroup = default;
            Optional<string> resourceProvider = default;
            Optional<string> resourceUri = default;
            Optional<string> operationName = default;
            Optional<string> status = default;
            Optional<string> authorization = default;
            Optional<string> claims = default;
            Optional<string> correlationId = default;
            Optional<string> httpRequest = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("tenantId"))
                {
                    tenantId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("subscriptionId"))
                {
                    subscriptionId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("resourceGroup"))
                {
                    resourceGroup = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("resourceProvider"))
                {
                    resourceProvider = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("resourceUri"))
                {
                    resourceUri = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("operationName"))
                {
                    operationName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("status"))
                {
                    status = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("authorization"))
                {
                    authorization = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("claims"))
                {
                    claims = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("correlationId"))
                {
                    correlationId = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("httpRequest"))
                {
                    httpRequest = property.Value.GetString();
                    continue;
                }
            }
            return new ResourceDeleteFailureData(tenantId.Value, subscriptionId.Value, resourceGroup.Value, resourceProvider.Value, resourceUri.Value, operationName.Value, status.Value, authorization.Value, claims.Value, correlationId.Value, httpRequest.Value);
        }
    }
}
