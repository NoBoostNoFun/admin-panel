using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AdminPanel.DummyApi;

public interface IDummyApiClient
{
    Task<DummyApiResponse<DummyApiUser[]>> GetUsers(PageInfo pageInfo, CancellationToken cancellationToken);
    Task<DummyApiResponse<DummyApiPost[]>> GetUserPosts(string userId, PageInfo pageInfo, CancellationToken cancellationToken);
}

[UsedImplicitly]
public record DummyApiResponse<T>(
    T Data,
    int Total,
    int Page,
    int Limit
);

[UsedImplicitly]
public record DummyApiUser(
    string Id,
    string Title,
    string FirstName,
    string LastName,
    string Picture
);

[UsedImplicitly]
public record DummyApiPost(
    string Id,
    string Text,
    string Image,
    int Likes,
    string[] Tags,
    string PublishDate,
    DummyApiUser Owner
);

public record PageInfo(int Page, int Limit)
{
    public string ToQuery()
        => $"page={Page}&limit={Limit}";
}
