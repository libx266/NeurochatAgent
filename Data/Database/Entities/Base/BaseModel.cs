using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Database.Entities;
using Data.Database.Entities.Enums;

namespace Data.Database.Entities.Base
{
    public abstract class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("external_id")]
        public long? ExternalId { get; set; }

        [Column("insert_date")]
        public DateTime InsertDate { get; set; } = DateTime.UtcNow;

        [Column("api_service")]
        public ApiService ApiService { get; set; }

        [Column("additional_info")]
        [MaxLength(1024)]
        public string? AdditionalInfo { get; set; }
    }
}
