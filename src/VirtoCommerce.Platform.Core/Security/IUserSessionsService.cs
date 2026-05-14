using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

public interface IUserSessionsService
{
    Task TerminateUserSession(string sessionId);
    Task TerminateUserSessionGroup(string sessionGroupId);
    Task TerminateAllUserSessions(string userId);
}
