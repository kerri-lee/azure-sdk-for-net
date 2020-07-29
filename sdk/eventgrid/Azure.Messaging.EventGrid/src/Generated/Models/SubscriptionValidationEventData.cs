// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Messaging.EventGrid.Models
{
    /// <summary> Schema of the Data property of an EventGridEvent for a Microsoft.EventGrid.SubscriptionValidationEvent. </summary>
    public partial class SubscriptionValidationEventData
    {
        /// <summary> Initializes a new instance of SubscriptionValidationEventData. </summary>
        internal SubscriptionValidationEventData()
        {
        }

        /// <summary> The validation code sent by Azure Event Grid to validate an event subscription. To complete the validation handshake, the subscriber must either respond with this validation code as part of the validation response, or perform a GET request on the validationUrl (available starting version 2018-05-01-preview). </summary>
        public string ValidationCode { get; }
        /// <summary> The validation URL sent by Azure Event Grid (available starting version 2018-05-01-preview). To complete the validation handshake, the subscriber must either respond with the validationCode as part of the validation response, or perform a GET request on the validationUrl (available starting version 2018-05-01-preview). </summary>
        public string ValidationUrl { get; }
    }
}
