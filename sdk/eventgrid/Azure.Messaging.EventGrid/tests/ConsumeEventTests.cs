// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.EventGrid.Tests
{
    public class ConsumeEventTests
    {
        public readonly EventGridConsumer _eventGridConsumer;

        public ConsumeEventTests()
        {
            _eventGridConsumer = new EventGridConsumer();
        }
    }
}
