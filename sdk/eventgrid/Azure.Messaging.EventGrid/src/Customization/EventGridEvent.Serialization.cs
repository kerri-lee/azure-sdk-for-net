// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using Azure.Core;

namespace Azure.Messaging.EventGrid.Models
{
    public partial class EventGridEvent
    {
        internal static EventGridEvent DeserializeEventGridEvent(JsonElement element)
        {
            string id = default;
            Optional<string> topic = default;
            string subject = default;
            object data = default;
            string eventType = default;
            DateTimeOffset eventTime = default;
            Optional<string> metadataVersion = default;
            string dataVersion = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("topic"))
                {
                    topic = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("subject"))
                {
                    subject = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("data"))
                {
                    data = property.Value;
                    continue;
                }
                if (property.NameEquals("eventType"))
                {
                    eventType = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("eventTime"))
                {
                    eventTime = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("metadataVersion"))
                {
                    metadataVersion = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("dataVersion"))
                {
                    dataVersion = property.Value.GetString();
                    continue;
                }
            }
            return new EventGridEvent(id, topic.Value, subject, data, eventType, eventTime, metadataVersion.Value, dataVersion);
        }
    }
}
