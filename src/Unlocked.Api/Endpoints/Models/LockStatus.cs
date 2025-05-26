using System.Text.Json.Serialization;

namespace Unlocked.Api.Endpoints.Models;

public enum LockStatus
{
    [JsonStringEnumMemberName("locked")]
    Locked,

    [JsonStringEnumMemberName("unlocked")]
    Unlocked,

    [JsonStringEnumMemberName("unknown")]
    Unknown
}