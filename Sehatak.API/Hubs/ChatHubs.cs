using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Sehatak.API.Hubs;

// بس الطاقم الطبي يقدر يستخدم الشات
[Authorize(Policy = "MedicalStaff")]
public class ChatHubs : Hub
{
    // لما موظف يبعت رسالة
    public async Task SendMessage(int receiverId, string message)
    {
        // بنجيب Id المرسل من الـ JWT Token
        var senderId = Context.UserIdentifier;

        // بنبعت الرسالة للمستلم بس
        await Clients.User(receiverId.ToString())
            .SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                Message = message,
                SentAt = DateTime.UtcNow
            });
    }

    // لما موظف يفتح الشات — بنعلم الكل إنه أونلاين
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        await Clients.Others.SendAsync("UserOnline", userId);
        await base.OnConnectedAsync();
    }

    // لما موظف يغلق الشات — بنعلم الكل إنه أوفلاين
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        await Clients.Others.SendAsync("UserOffline", userId);
        await base.OnDisconnectedAsync(exception);
    }
}
