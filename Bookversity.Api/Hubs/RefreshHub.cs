using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Bookversity.Api.Hubs
{
    public class RefreshHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public Task Refresh()
        {
            return Clients.Others.SendAsync("refresh");
        }
    }
}
