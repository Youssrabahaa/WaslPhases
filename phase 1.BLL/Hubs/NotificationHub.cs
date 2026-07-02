using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace phase_1.BLL.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinUserGroup(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        }

        public async Task LeaveUserGroup(int userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
        }
    }
}