using System;
using System.Threading;
using System.Threading.Tasks;
using AdminPanel.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace AdminPanel.Cache;

[UsedImplicitly]
public class UserServiceCacheDecorator : IUserService
{
    private readonly IUserService _target;
    private readonly IDistributedCache _cache;
    private readonly UserServiceCacheOptions _options;

    public UserServiceCacheDecorator(
        IUserService target,
        IDistributedCache cache,
        IOptions<UserServiceCacheOptions> options)
    {
        _target = target;
        _cache = cache;
        _options = options.Value;
    }

    public async Task<User[]> GetAllUsers(CancellationToken cancellationToken)
        => await GetOrUpdateAsync(
            "all-users",
            () => _target.GetAllUsers(cancellationToken),
            _options.UsersExpirationTimeMin
        );

    public async Task<UserPost[]> GetUserPosts(string userId, CancellationToken cancellationToken)
        => await GetOrUpdateAsync(
            $"user-posts:{userId}",
            () => _target.GetUserPosts(userId, cancellationToken),
            _options.UserPostsExpirationTimeMin
        );

    private async Task<T> GetOrUpdateAsync<T>(string key, Func<Task<T>> get, int expirationTimeMin) where T : class
    {
        if (!_options.IsEnabled)
            return await get();

        var data = await _cache.GetAsync<T>(key);
        if (data is not null)
            return data;

        var result = await get();
        await _cache.SetAsync(key, result, expirationTimeMin);
        return result;
    }
}
