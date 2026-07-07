using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Sehatak.API.Hubs;

[Authorize(Policy = "MedicalStaff")]
public class ChatHubs : Hub
{
    public async Task SendMessage(int receiverId, string message)
    {

        var senderId = Context.UserIdentifier;


        await Clients.User(receiverId.ToString())
            .SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                Message = message,
                SentAt = DateTime.UtcNow
            });
    }


    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        await Clients.Others.SendAsync("UserOnline", userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        await Clients.Others.SendAsync("UserOffline", userId);
        await base.OnDisconnectedAsync(exception);
    }
}
