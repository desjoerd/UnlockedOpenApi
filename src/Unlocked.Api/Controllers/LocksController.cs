using Microsoft.AspNetCore.Mvc;

using Unlocked.Api.Controllers.Models;

namespace Unlocked.Api.Controllers;

[ApiController]
[Route("/locks")]
public class LocksController : ControllerBase
{
    /// <summary>
    /// Documentation about this endpoint
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<List<LockListItem>> ListLocks()
    {
        var locks = new List<LockListItem>() {
            new LockListItem { Id = "123" }
        };

        return locks;
    }
}