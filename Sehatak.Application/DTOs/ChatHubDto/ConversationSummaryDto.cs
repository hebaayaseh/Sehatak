using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.ChatHubDto
{
    public class ConversationSummaryDto
    {
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; } = string.Empty;
        public string? OtherUserProfileImage { get; set; }
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageAt { get; set; }
        public bool LastMessageIsRead { get; set; }
        public int LastMessageSenderId { get; set; }
        public int UnreadCount { get; set; }
    }
}
