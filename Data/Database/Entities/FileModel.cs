using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Database.Entities.Base;
using Data.Database.Entities.Enums;

namespace Data.Database.Entities
{
    [Table("files")]
    public sealed class FileModel : BaseModel
    {
        public const int MiB = 1048576;


        [Column("name")]
        [MaxLength(byte.MaxValue)]
        public string Name { get; set; }

        [Column("path")]
        [MaxLength(byte.MaxValue)]
        public string? Path { get; set; }

        [Column("data")]
        [MaxLength(16 * MiB)]
        public byte[]? Data { get; set; }

        [Column("attach_type")]
        public AttachType AttachType { get; set; }

        [Column("message_id")]
        [ForeignKey("messages")]
        public int? MessageId { get; set; }

        public MessageModel? Message { get; set; }

        [NotMapped]
        public byte[] LocalContent
        {
            get
            {
                if (Path != default)
                {
                    return System.IO.File.ReadAllBytes(Path);
                }

                throw new InvalidOperationException("File is empty");
            }
            set
            {
                if (Path != default)
                {
                    System.IO.File.WriteAllBytes(Path, value);
                    return;
                }

                throw new InvalidOperationException("Path not specified");
            }
        }
    }
}
