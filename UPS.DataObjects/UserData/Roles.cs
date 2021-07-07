using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPS.DataObjects.UserData
{
    [Table("RLE")]
    public class UpsRoles
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("RLE-NA")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("RLE-IS-ACT-B")]
        public bool IsActive { get; set; }
        [Column("CRT-DT")]
        public DateTime CreatedDate { get; set; }
        [Column("UDT-DT")]
        public DateTime UpdatedDate { get; set; }
        [Column("CRT-BY-NR")]
        public int CreatedBy { get; set; }
        [Column("UDT-BY-NR")]
        public int UpdatedBy { get; set; }
    }
}
