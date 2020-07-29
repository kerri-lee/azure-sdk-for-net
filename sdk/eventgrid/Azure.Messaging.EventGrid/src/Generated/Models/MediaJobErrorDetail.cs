// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Messaging.EventGrid.Models
{
    /// <summary> Details of JobOutput errors. </summary>
    public partial class MediaJobErrorDetail
    {
        /// <summary> Initializes a new instance of MediaJobErrorDetail. </summary>
        internal MediaJobErrorDetail()
        {
        }

        /// <summary> Initializes a new instance of MediaJobErrorDetail. </summary>
        /// <param name="code"> Code describing the error detail. </param>
        /// <param name="message"> A human-readable representation of the error. </param>
        internal MediaJobErrorDetail(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary> Code describing the error detail. </summary>
        public string Code { get; }
        /// <summary> A human-readable representation of the error. </summary>
        public string Message { get; }
    }
}
