// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using Azure.Core;

namespace Azure.Messaging.EventGrid
{
    internal class EventGridSerializer : IUtf8JsonSerializable
    {
        public BinaryData _customEvent;
        public CancellationToken _cancellationToken;
        public ObjectSerializer _serializer;

        public EventGridSerializer(BinaryData customEvent, ObjectSerializer serializer, CancellationToken cancellationToken)
        {
            _customEvent = customEvent;
            _serializer = serializer;
            _cancellationToken = cancellationToken;
        }
        public void Write(Utf8JsonWriter writer)
        {
            var stream = new MemoryStream();
            if (!_customEvent.IsSerialized)
            {
                _serializer.Serialize(stream, _customEvent.ToString(), typeof(string), _cancellationToken);
                stream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                stream = (MemoryStream)_customEvent.ToStream();
            }
            JsonDocument.Parse(stream).WriteTo(writer);
        }
    }
}
