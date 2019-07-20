using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPS.DataObjects.UserData
{
    [Table("USR")]
    public class User
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        [Column("USR-FST-NA")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column("USR-LST-NA")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Column("USR-EML-TE")]
        [StringLength(50)]
        public string Email { get; set; }
        [Column("USR-PWD-TE")]
        [StringLength(50)]
        public string Password { get; set; }
        [Column("USR-PWD-HSH-TE")]
        [StringLength(500)]
        public string PasswordHashTag { get; set; }
        [Column("USR-CRT-DT")]
        public DateTime? CreatedDate { get; set; }
        [Column("USR-UDT-DT")]
        public DateTime? UpdatedDate { get; set; }
        [Column("USR-CRT-BY-NR")]
        public int? CreatedBy { get; set; }
        [Column("USR-UDT-BY-NR")]
        public int? UpdatedBy { get; set; }
        [Column("IS-ACT-B")]
        public bool? IsActive { get; set; }
        [Column("USR-ID-TE")]
        [StringLength(10)]
        public string UserId { get; set; }
        [NotMapped]
        public List<string> Cities { get; set; }

        [NotMapped]
        public int Role { get; set; }
    }
}
