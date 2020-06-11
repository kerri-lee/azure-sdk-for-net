// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Usage",
    "AZC0001:Use one of the following pre-approved namespace groups (https://azure.github.io/azure-sdk/registered_namespaces.html): Azure.AI, Azure.Analytics, Azure.Data, Azure.DigitalTwins, Azure.Iot, Azure.Media, Azure.Management, Azure.Messaging, Azure.Search, Azure.Security, Azure.Storage, Azure.Template, Azure.Identity, Microsoft.Extensions.Azure",
    Justification = "<Pending>",
    Scope = "module")]
[assembly: SuppressMessage("Usage", "AZC0008:ClientOptions should have a nested enum called ServiceVersion", Justification = "<Pending>", Scope = "type", Target = "~T:Azure.ResourceManager.DigitalTwins.DigitalTwinsManagementClientOptions")]
