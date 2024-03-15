using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Database.Entities.Base;

namespace Data.Database.Entities
{
    [Table("chats")]
    public sealed class ChatModel : WithPhoto
    {
        [Column("name")]
        [MaxLength(byte.MaxValue)]
        public string Name { get; set; }
    }
}
