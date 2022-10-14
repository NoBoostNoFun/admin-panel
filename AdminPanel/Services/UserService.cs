using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.DummyApi;
using Microsoft.Extensions.Options;

namespace AdminPanel.Services;

public class UserService : IUserService
{
    private readonly IDummyApiClient _client;
    private readonly UserServiceOptions _options;

    public UserService(IDummyApiClient client, IOptions<UserServiceOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<User[]> GetAllUsers(CancellationToken ct)
    {
        var users = await GetAllPages(p => _client.GetUsers(p, ct), _options.FetchUsersLimit);
        return users
            .DistinctBy(d => d.Id)
            .Select(s => new User(s.Id, $"{s.FirstName} {s.LastName}", s.Picture))
            .ToArray();
    }

    public async Task<UserPost[]> GetUserPosts(string userId, CancellationToken ct)
    {
        var posts = await GetAllPages(p => _client.GetUserPosts(userId, p, ct), _options.FetchUserPostsLimit);
        return posts
            .Select(s => new UserPost(
                s.Text,
                s.Image,
                s.Tags,
                DateTime.Parse(s.PublishDate),
                $"{s.Owner.FirstName} {s.Owner.LastName}"))
            .ToArray();
    }

    private static async Task<List<T>> GetAllPages<T>(
        Func<PageInfo, Task<DummyApiResponse<T[]>>> getOnePage,
        int fetchLimit)
    {
        List<T> users = new();
        DummyApiResponse<T[]> response;
        var page = 1;

        do
        {
            var pageInfo = new PageInfo(page++, fetchLimit);
            response = await getOnePage(pageInfo);
            users.AddRange(response.Data);
        }
        while (response.Page * response.Limit < response.Total);

        return users;
    }
}
