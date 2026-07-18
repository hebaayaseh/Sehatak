using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.ChatHubDto
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int SenterId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public bool isRead { get; set; }
        public bool IsDeleted {  get; set; }
        public DateTime SendAt { get; set; }
    }
}
