// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using Azure.Core;

namespace Azure.Messaging.EventGrid.Models
{
    /// <summary> Properties of an event published to an Event Grid topic using the CloudEvent 1.0 Schema. </summary>
    public partial class CloudEvent : IDictionary<string, object>
    {
        /// <summary> Initializes a new instance of CloudEvent. </summary>
        /// <param name="source"> Identifies the context in which an event happened. The combination of id and source must be unique for each distinct event. </param>
        /// <param name="type"> Type of event related to the originating occurrence. </param>
        public CloudEvent(string source, string type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Source = source;
            Type = type;
            AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary> Initializes a new instance of CloudEventInternal. </summary>
        /// <param name="id"> An identifier for the event. The combination of id and source must be unique for each distinct event. </param>
        /// <param name="source"> Identifies the context in which an event happened. The combination of id and source must be unique for each distinct event. </param>
        /// <param name="data"> Event data specific to the event type. </param>
        /// <param name="type"> Type of event related to the originating occurrence. </param>
        /// <param name="time"> The time (in UTC) the event was generated, in RFC3339 format. </param>
        /// <param name="specversion"> The version of the CloudEvents specification which the event uses. </param>
        /// <param name="dataschema"> Identifies the schema that data adheres to. </param>
        /// <param name="datacontenttype"> Content type of data value. </param>
        /// <param name="subject"> This describes the subject of the event in the context of the event producer (identified by source). </param>
        /// <param name="additionalProperties"> . </param>
        internal CloudEvent(string id, string source, object data, string type, DateTimeOffset? time, string specversion, string dataschema, string datacontenttype, string subject, IDictionary<string, object> additionalProperties)
        {
            Id = id;
            Source = source;
            Data = data;
            Type = type;
            Time = time;
            SpecVersion = specversion;
            DataSchema = dataschema;
            DataContentType = datacontenttype;
            Subject = subject;
            AdditionalProperties = additionalProperties;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudEvent"/> class.
        /// </summary>
        internal CloudEvent()
        {
        }

        /// <summary> An identifier for the event. The combination of id and source must be unique for each distinct event. </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary> Identifies the context in which an event happened. The combination of id and source must be unique for each distinct event. </summary>
        public string Source { get; set; }
        /// <summary> Event data specific to the event type. </summary>
        public object Data { get; set; }
        /// <summary> Type of event related to the originating occurrence. </summary>
        public string Type { get; set; }
        /// <summary> The time (in UTC) the event was generated, in RFC3339 format. </summary>
        public DateTimeOffset? Time { get; set; }
        /// <summary> The version of the CloudEvents specification which the event uses. </summary>
        public string SpecVersion { get; internal set; } = "1.0";
        /// <summary> Identifies the schema that data adheres to. </summary>
        public string DataSchema { get; set; }
        /// <summary> Content type of data value. </summary>
        public string DataContentType { get; set; }
        /// <summary> This describes the subject of the event in the context of the event producer (identified by source). </summary>
        public string Subject { get; set; }
        internal IDictionary<string, object> AdditionalProperties { get; set; }
        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => AdditionalProperties.GetEnumerator();
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => AdditionalProperties.GetEnumerator();
        /// <inheritdoc />
        public bool TryGetValue(string key, out object value) => AdditionalProperties.TryGetValue(key, out value);
        /// <inheritdoc />
        public bool ContainsKey(string key) => AdditionalProperties.ContainsKey(key);
        /// <inheritdoc />
        public ICollection<string> Keys => AdditionalProperties.Keys;
        /// <inheritdoc />
        public ICollection<object> Values => AdditionalProperties.Values;
        /// <inheritdoc />
        int ICollection<KeyValuePair<string, object>>.Count => AdditionalProperties.Count;
        /// <inheritdoc />
        public void Add(string key, object value) => AdditionalProperties.Add(key, value);
        /// <inheritdoc />
        public bool Remove(string key) => AdditionalProperties.Remove(key);
        /// <inheritdoc />
        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => AdditionalProperties.IsReadOnly;
        /// <inheritdoc />
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> value) => AdditionalProperties.Add(value);
        /// <inheritdoc />
        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> value) => AdditionalProperties.Remove(value);
        /// <inheritdoc />
        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> value) => AdditionalProperties.Contains(value);
        /// <inheritdoc />
        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] destination, int offset) => AdditionalProperties.CopyTo(destination, offset);
        /// <inheritdoc />
        void ICollection<KeyValuePair<string, object>>.Clear() => AdditionalProperties.Clear();
        /// <inheritdoc />
        public object this[string key]
        {
            get => AdditionalProperties[key];
            set => AdditionalProperties[key] = value;
        }
    }
}
