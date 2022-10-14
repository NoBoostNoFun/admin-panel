using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AdminPanel.Services;

public interface IUserService
{
    Task<User[]> GetAllUsers(CancellationToken cancellationToken);
    Task<UserPost[]> GetUserPosts(string userId, CancellationToken cancellationToken);
}

[UsedImplicitly]
public record User(
    string Id,
    string Name,
    string Picture
);

[UsedImplicitly]
public record UserPost(
    string Text,
    string Image,
    string[] Tags,
    DateTime PublishDate,
    string OwnerName
);
