﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenAI.Realtime
{
    public sealed class RealtimeResponseResource
    {
        /// <summary>
        /// The unique ID of the response.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, must be "realtime.response".
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Object { get; private set; }

        /// <summary>
        /// The status of the response ("in_progress").
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonConverter(typeof(Extensions.JsonStringEnumConverter<RealtimeResponseStatus>))]
        public RealtimeResponseStatus Status { get; private set; }

        /// <summary>
        /// Additional details about the status.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("status_details")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public StatusDetails StatusDetails { get; private set; }

        /// <summary>
        /// The list of output items generated by the response.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("output")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IReadOnlyList<ConversationItem> Output { get; private set; }

        /// <summary>
        /// Usage statistics for the Response, this will correspond to billing.
        /// A Realtime API session will maintain a conversation context and append new Items to the Conversation,
        /// thus output from previous turns (text and audio tokens) will become the input for later turns.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("usage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Usage Usage { get; private set; }
    }
}
