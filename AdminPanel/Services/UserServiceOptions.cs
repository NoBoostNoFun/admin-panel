using JetBrains.Annotations;

namespace AdminPanel.Services;

public class UserServiceOptions
{
    public int FetchUsersLimit { get; [UsedImplicitly] init; } = 10;
    public int FetchUserPostsLimit { get; [UsedImplicitly] init; } = 10;
}
