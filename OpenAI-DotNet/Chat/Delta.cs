// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenAI.Chat
{
    public sealed class Delta
    {
        /// <summary>
        /// The <see cref="OpenAI.Role"/> of the author of this message.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("role")]
        public Role Role { get; private set; }

        /// <summary>
        /// The contents of the message.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("content")]
        public string Content { get; private set; }

        /// <summary>
        /// The refusal message generated by the model.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("refusal")]
        public string Refusal { get; private set; }

        /// <summary>
        /// The tool calls generated by the model, such as function calls.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("tool_calls")]
        public IReadOnlyList<ToolCall> ToolCalls { get; private set; }

        /// <summary>
        /// If the audio output modality is requested, this object contains data about the audio response from the model. 
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("audio")]
        public AudioOutput AudioOutput { get; private set; }

        /// <summary>
        /// Optional, The name of the author of this message.<br/>
        /// May contain a-z, A-Z, 0-9, and underscores, with a maximum length of 64 characters.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("name")]
        public string Name { get; private set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Content))
            {
                return AudioOutput?.ToString() ?? string.Empty;
            }

            return Content ?? string.Empty;
        }

        public static implicit operator string(Delta delta) => delta?.ToString();
    }
}
