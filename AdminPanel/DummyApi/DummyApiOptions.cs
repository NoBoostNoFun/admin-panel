using JetBrains.Annotations;

namespace AdminPanel.DummyApi;

public class DummyApiOptions
{
    public string AppId { get; [UsedImplicitly] init; } = default!;
    public string BaseUrl { get; [UsedImplicitly] init; } = default!;
}
