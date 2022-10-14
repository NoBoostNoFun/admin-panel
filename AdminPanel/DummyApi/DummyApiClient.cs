using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.Exceptions;

namespace AdminPanel.DummyApi;

public class DummyApiClient : IDummyApiClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public DummyApiClient(HttpClient client)
        => _client = client;

    public async Task<DummyApiResponse<DummyApiUser[]>> GetUsers(PageInfo pageInfo, CancellationToken ct)
        => await GetAsync<DummyApiResponse<DummyApiUser[]>>($"user?{pageInfo.ToQuery()}", ct);

    public async Task<DummyApiResponse<DummyApiPost[]>> GetUserPosts(string userId, PageInfo pageInfo, CancellationToken ct)
        => await GetAsync<DummyApiResponse<DummyApiPost[]>>($"user/{userId}/post?{pageInfo.ToQuery()}", ct);

    private async Task<T> GetAsync<T>(string path, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.GetAsync(path, cancellationToken);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<T>(stream, _serializerOptions, cancellationToken)
                   ?? throw new Exception("Couldn't deserialize response");
        }
        catch (Exception e)
        {
            throw new DummyApiIntegrationException(e);
        }
    }
}
