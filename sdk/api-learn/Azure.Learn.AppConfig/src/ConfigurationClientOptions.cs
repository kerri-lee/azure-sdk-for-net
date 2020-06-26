﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Azure.Core;

public class ConfigurationClientOptions : ClientOptions
{
    internal string Version { get; }

    public ConfigurationClientOptions(ServiceVersion version = ServiceVersion.V1_0)
    {
        Version = version switch
        {
            ServiceVersion.V1_0 => "1.0",
            _ => throw new ArgumentException($"The service version {version} is not supported by this library.", nameof(version))
        };
    }
    public enum ServiceVersion
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
        V1_0 = 0
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
