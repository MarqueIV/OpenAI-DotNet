﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace OpenAI.Responses
{
    public class FunctionToolCallOutput : BaseResponse, IResponseItem
    {
        public FunctionToolCallOutput() { }

        public FunctionToolCallOutput(FunctionToolCall toolCall, string output)
        {
            CallId = toolCall.CallId;
            Output = output;
        }

        public FunctionToolCallOutput(string callId, string output)
        {
            CallId = callId;
            Output = output;
        }

        /// <inheritdoc />
        [JsonInclude]
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; private set; }

        /// <inheritdoc />
        [JsonInclude]
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ResponseItemType Type { get; private set; } = ResponseItemType.FunctionCallOutput;

        /// <inheritdoc />
        [JsonInclude]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Object { get; private set; }

        /// <inheritdoc />
        [JsonInclude]
        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ResponseStatus Status { get; private set; }

        /// <summary>
        /// The unique ID of the function tool call generated by the model.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("call_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string CallId { get; private set; }

        /// <summary>
        /// A JSON string of the output of the function tool call.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("output")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Output { get; private set; }

        public override string ToString()
            => Output;
    }

    public class FunctionToolCallOutput<T> : FunctionToolCallOutput
    {
        public FunctionToolCallOutput(FunctionToolCall toolCall, T outputResults, string output)
            : base(toolCall, output)
        {
            OutputResult = outputResults;
        }

        [JsonIgnore]
        public T OutputResult { get; internal set; }
    }
}
