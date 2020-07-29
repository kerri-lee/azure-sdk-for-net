// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Azure.Messaging.EventGrid.Models
{
    /// <summary> Job output processing event data. </summary>
    public partial class MediaJobOutputProcessingEventData : MediaJobOutputStateChangeEventData
    {
        /// <summary> Initializes a new instance of MediaJobOutputProcessingEventData. </summary>
        internal MediaJobOutputProcessingEventData()
        {
        }

        /// <summary> Initializes a new instance of MediaJobOutputProcessingEventData. </summary>
        /// <param name="previousState"> The previous state of the Job. </param>
        /// <param name="output"> Gets the output. </param>
        /// <param name="jobCorrelationData"> Gets the Job correlation data. </param>
        internal MediaJobOutputProcessingEventData(MediaJobState? previousState, MediaJobOutput output, IReadOnlyDictionary<string, string> jobCorrelationData) : base(previousState, output, jobCorrelationData)
        {
        }
    }
}
