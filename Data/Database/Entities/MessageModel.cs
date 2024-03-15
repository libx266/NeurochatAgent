using Data.Database.Entities.Base;
using Data.Database.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Database.Entities
{
    [Table("messages")]
    public class MessageModel : BaseModel
    {
        [Column("text")]
        [MaxLength(ushort.MaxValue)]
        public string? Text { get; set; }

        [Column("sender_date")]
        public DateTime SenderDate { get; set; }

        [Column("sender_id")]
        [ForeignKey("users")]
        public int SenderId { get; set; }

        public UserModel Sender { get; set; }

        [Column("message_type")]
        public MessageType MessageType { get; set; } = MessageType.Chat;

        [ForeignKey("messages")]
        [Column("replay_message_id")]
        public int? ReplayMessageId { get; set; }

        public MessageModel? ReplayMessage { get; set; }

        [ForeignKey("chats")]
        [Column("chat_id")]
        public int ChatId { get; set; }

        public ChatModel Chat { get; set; }


       
        

    }
}
