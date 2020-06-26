// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core;

namespace Azure.Learn.AppConfig.Models
{
    [CodeGenModel("KeyValue")]
    public partial class ConfigurationSetting
    {
        public string Key { get; set; }
        public string ContentType { get; set; }
        public string Value { get; set; }
    }
}
