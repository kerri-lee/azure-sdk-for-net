// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Azure.Messaging.EventGrid.Models
{
    public partial class CloudEvent
    {
        internal static CloudEvent DeserializeCloudEvent(JsonElement element)
        {
            string id = default;
            string source = default;
            Optional<object> data = default;
            //Optional<byte[]> dataBase64 = default;
            string type = default;
            Optional<DateTimeOffset> time = default;
            string specversion = default;
            Optional<string> dataschema = default;
            Optional<string> datacontenttype = default;
            Optional<string> subject = default;
            IDictionary<string, object> additionalProperties = default;
            Dictionary<string, object> additionalPropertiesDictionary = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("source"))
                {
                    source = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("data"))
                {
                    data = property.Value;
                    continue;
                }
                if (property.NameEquals("data_base64"))
                {
                    data = Convert.FromBase64String(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("type"))
                {
                    type = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("time"))
                {
                    time = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("specversion"))
                {
                    specversion = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("dataschema"))
                {
                    dataschema = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("datacontenttype"))
                {
                    datacontenttype = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("subject"))
                {
                    subject = property.Value.GetString();
                    continue;
                }
                additionalPropertiesDictionary ??= new Dictionary<string, object>();
                additionalPropertiesDictionary.Add(property.Name, property.Value.GetObject());
            }
            additionalProperties = additionalPropertiesDictionary;
            return new CloudEvent(id, source, data.Value, type, Optional.ToNullable(time), specversion, dataschema.Value, datacontenttype.Value, subject.Value, additionalProperties);
        }
    }
}
