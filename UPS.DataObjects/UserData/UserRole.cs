using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.UserData
{
    [Table("USR-IN-RLE")]
    public class UserRole
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("USR-ID")]
        public int UserId { get; set; }
        [Column("USR-RLE-ID-NR")]
        public int RoleId { get; set; }
        [Column("IS-ACT-B")]
        public bool? IsAcive { get; set; }

    }
}
