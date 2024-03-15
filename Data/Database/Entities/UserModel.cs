using Data.Database.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Database.Entities.Enums;

namespace Data.Database.Entities
{
    [Table("users")]
    public sealed class UserModel : WithPhoto
    {
        [Column("first_name")]
        [MaxLength(byte.MaxValue)]
        public string FirstName { get; set; }

        [Column("last_name")]
        [MaxLength(byte.MaxValue)]
        public string LastName { get; set; }

        [Column("username")]
        [MaxLength(byte.MaxValue)]
        public string? Username { get; set; }

        [Column("user_type")]
        public UserType UserType { get; set; }
    }
}
