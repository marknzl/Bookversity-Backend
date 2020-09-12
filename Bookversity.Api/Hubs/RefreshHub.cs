﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookversity.Api.Hubs
{
    public class RefreshHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //string clientIp = Context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            //await Clients.Others.SendAsync("RefreshFromUser", clientIp);

            await base.OnConnectedAsync();
        }

        public Task Refresh()
        {
            return Clients.Others.SendAsync("refresh");
        }
    }
}