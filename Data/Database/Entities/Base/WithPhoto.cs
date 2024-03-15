using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Database.Entities.Base
{
    public abstract class WithPhoto : BaseModel
    {
        [Column("photo_id")]
        [ForeignKey("files")]
        public int? PhotoId { get; set; }

        public FileModel? Photo { get; set; }
    }
}
