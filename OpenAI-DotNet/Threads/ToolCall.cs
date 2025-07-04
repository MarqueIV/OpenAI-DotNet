// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Extensions;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI.Threads
{
    public sealed class ToolCall : IToolCall, IAppendable<ToolCall>
    {
        public ToolCall() { }

        public ToolCall(string callId, string name, string arguments = null)
        {
            Id = callId;
            Type = "function";
            FunctionCall = new FunctionCall(name, arguments);
        }

        [JsonInclude]
        [JsonPropertyName("index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? Index { get; private set; }

        /// <summary>
        /// The ID of the tool call.
        /// This ID must be referenced when you submit the tool outputs in using the Submit tool outputs to run endpoint.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The type of tool call the output is required for.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The definition of the function that was called.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("function")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FunctionCall FunctionCall { get; private set; }

        /// <summary>
        /// The Code Interpreter tool call definition.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("code_interpreter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CodeInterpreter CodeInterpreter { get; private set; }

        /// <summary>
        /// The File Search tool call definition.
        /// </summary>
        /// <remarks>
        /// For now, this is always going to be an empty object.
        /// </remarks>
        [JsonInclude]
        [JsonPropertyName("file_search")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IReadOnlyDictionary<string, object> FileSearch { get; private set; }

        [JsonIgnore]
        public bool IsFunction => Type == "function";

        [JsonIgnore]
        public string CallId => Id;

        [JsonIgnore]
        public string Name => FunctionCall?.Name;

        [JsonIgnore]
        public JsonNode Arguments => FunctionCall?.Arguments;

        public void AppendFrom(ToolCall other)
        {
            if (other == null) { return; }

            if (!string.IsNullOrWhiteSpace(other.Id))
            {
                Id = other.Id;
            }

            if (other.Index.HasValue)
            {
                Index = other.Index;
            }

            if (other.FunctionCall != null)
            {
                if (FunctionCall == null)
                {
                    FunctionCall = other.FunctionCall;
                }
                else
                {
                    FunctionCall.AppendFrom(other.FunctionCall);
                }
            }

            if (other.CodeInterpreter != null)
            {
                if (CodeInterpreter == null)
                {
                    CodeInterpreter = other.CodeInterpreter;
                }
                else
                {
                    CodeInterpreter.AppendFrom(other.CodeInterpreter);
                }
            }

            if (other.FileSearch != null)
            {
                FileSearch = other.FileSearch;
            }
        }

        public string InvokeFunction()
            => ToolExtensions.InvokeFunction(this);

        public T InvokeFunction<T>()
            => ToolExtensions.InvokeFunction<T>(this);

        public Task<string> InvokeFunctionAsync(CancellationToken cancellationToken = default)
            => ToolExtensions.InvokeFunctionAsync(this, cancellationToken);

        public Task<T> InvokeFunctionAsync<T>(CancellationToken cancellationToken = default)
            => ToolExtensions.InvokeFunctionAsync<T>(this, cancellationToken);
    }
}
