using JetBrains.Annotations;

namespace AdminPanel.Cache;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class UserServiceCacheOptions
{
    public bool IsEnabled { get; init; }
    public int UserPostsExpirationTimeMin { get; init; } = 10;
    public int UsersExpirationTimeMin { get; init; } = 10;
}
