// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Extensions;
using OpenAI.Files;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI.Assistants
{
    public sealed class AssistantsEndpoint : OpenAIBaseEndpoint
    {
        internal AssistantsEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "assistants";

        /// <summary>
        /// Get list of assistants.
        /// </summary>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{AssistantResponse}"/>.</returns>
        public async Task<ListResponse<AssistantResponse>> ListAssistantsAsync(ListQuery query = null, CancellationToken cancellationToken = default)
        {
            using var response = await client.Client.GetAsync(GetUrl(queryParameters: query), cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<ListResponse<AssistantResponse>>(responseAsString, client);
        }

        /// <summary>
        /// Create an assistant.
        /// </summary>
        /// <typeparam name="T"><see cref="JsonSchema"/> to use for structured outputs.</typeparam>
        /// <param name="request"><see cref="CreateAssistantRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantResponse"/>.</returns>
        public async Task<AssistantResponse> CreateAssistantAsync<T>(CreateAssistantRequest request = null, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                request = new CreateAssistantRequest(jsonSchema: typeof(T));
            }
            else
            {
                request.ResponseFormatObject = new ResponseFormatObject(typeof(T));
            }

            return await CreateAssistantAsync(request, cancellationToken);
        }

        /// <summary>
        /// Create an assistant.
        /// </summary>
        /// <param name="request"><see cref="CreateAssistantRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantResponse"/>.</returns>
        public async Task<AssistantResponse> CreateAssistantAsync(CreateAssistantRequest request = null, CancellationToken cancellationToken = default)
        {
            request ??= new CreateAssistantRequest();
            using var payload = JsonSerializer.Serialize(request, OpenAIClient.JsonSerializationOptions).ToJsonStringContent();
            using var response = await client.Client.PostAsync(GetUrl(), payload, cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, payload, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<AssistantResponse>(responseAsString, client);
        }

        /// <summary>
        /// Retrieves an assistant.
        /// </summary>
        /// <param name="assistantId">The ID of the assistant to retrieve.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantResponse"/>.</returns>
        public async Task<AssistantResponse> RetrieveAssistantAsync(string assistantId, CancellationToken cancellationToken = default)
        {
            using var response = await client.Client.GetAsync(GetUrl($"/{assistantId}"), cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<AssistantResponse>(responseAsString, client);
        }

        /// <summary>
        /// Modifies an assistant.
        /// </summary>
        /// <param name="assistantId">The ID of the assistant to modify.</param>
        /// <param name="request"><see cref="CreateAssistantRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantResponse"/>.</returns>
        public async Task<AssistantResponse> ModifyAssistantAsync(string assistantId, CreateAssistantRequest request, CancellationToken cancellationToken = default)
        {
            using var payload = JsonSerializer.Serialize(request, OpenAIClient.JsonSerializationOptions).ToJsonStringContent();
            using var response = await client.Client.PostAsync(GetUrl($"/{assistantId}"), payload, cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, payload, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<AssistantResponse>(responseAsString, client);
        }

        /// <summary>
        /// Delete an assistant.
        /// </summary>
        /// <param name="assistantId">The ID of the assistant to delete.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if the assistant was deleted.</returns>
        public async Task<bool> DeleteAssistantAsync(string assistantId, CancellationToken cancellationToken = default)
        {
            using var response = await client.Client.DeleteAsync(GetUrl($"/{assistantId}"), cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<DeletedResponse>(responseAsString, client)?.Deleted ?? false;
        }

        #region Files (Obsolete)

        /// <summary>
        /// Returns a list of assistant files.
        /// </summary>
        /// <param name="assistantId">The ID of the assistant the file belongs to.</param>
        /// <param name="query"><see cref="ListQuery"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ListResponse{AssistantFile}"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public async Task<ListResponse<AssistantFileResponse>> ListFilesAsync(string assistantId, ListQuery query = null, CancellationToken cancellationToken = default)
        {
            using var response = await client.Client.GetAsync(GetUrl($"/{assistantId}/files", query), cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<ListResponse<AssistantFileResponse>>(responseAsString, client);
        }

        /// <summary>
        /// Attach a file to an assistant.
        /// </summary>
        /// <param name="assistantId"> The ID of the assistant for which to attach a file. </param>
        /// <param name="file">
        /// A <see cref="FileResponse"/> (with purpose="assistants") that the assistant should use.
        /// Useful for tools like retrieval and code_interpreter that can access files.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantFileResponse"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public async Task<AssistantFileResponse> AttachFileAsync(string assistantId, FileResponse file, CancellationToken cancellationToken = default)
        {
            if (file?.Purpose?.Equals(FilePurpose.Assistants) != true)
            {
                throw new InvalidOperationException($"{nameof(file)}.{nameof(file.Purpose)} must be 'assistants'!");
            }

            using var payload = JsonSerializer.Serialize(new { file_id = file.Id }, OpenAIClient.JsonSerializationOptions).ToJsonStringContent();
            using var response = await client.Client.PostAsync(GetUrl($"/{assistantId}/files"), payload, cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, payload, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<AssistantFileResponse>(responseAsString, client);
        }

        /// <summary>
        /// Retrieves an AssistantFile.
        /// </summary>
        /// <param name="assistantId">The ID of the assistant who the file belongs to.</param>
        /// <param name="fileId">The ID of the file we're getting.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AssistantFileResponse"/>.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public async Task<AssistantFileResponse> RetrieveFileAsync(string assistantId, string fileId, CancellationToken cancellationToken = default)
        {
            using var response = await client.Client.GetAsync(GetUrl($"/{assistantId}/files/{fileId}"), cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<AssistantFileResponse>(responseAsString, client);
        }

        /// <summary>
        /// Remove an assistant file.
        /// </summary>
        /// <remarks>
        /// Note that removing an AssistantFile does not delete the original File object,
        /// it simply removes the association between that File and the Assistant.
        /// To delete a File, use the File delete endpoint instead.
        /// </remarks>
        /// <param name="assistantId">The ID of the assistant that the file belongs to.</param>
        /// <param name="fileId">The ID of the file to delete.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if file was removed.</returns>
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public async Task<bool> RemoveFileAsync(string assistantId, string fileId, CancellationToken cancellationToken = default)
        {
            using var response = await client.Client.DeleteAsync(GetUrl($"/{assistantId}/files/{fileId}"), cancellationToken).ConfigureAwait(false);
            var responseAsString = await response.ReadAsStringAsync(EnableDebug, cancellationToken).ConfigureAwait(false);
            return response.Deserialize<DeletedResponse>(responseAsString, client)?.Deleted ?? false;
        }

        #endregion Files (Obsolete)
    }
}
