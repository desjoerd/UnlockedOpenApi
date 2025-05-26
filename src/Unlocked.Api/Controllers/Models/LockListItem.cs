using Unlocked.Api.Endpoints.Models;

namespace Unlocked.Api.Controllers.Models;

public class LockListItem
{
    public required Guid Id { get; set; }

    public LockStatus Status { get; set; }
}