using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UPS.DataObjects.UserData
{
    [Table("USR-CTY-MPG")]
    public class UserCityMapping
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("USR-ID")]
        public int UserId { get; set; }
        [Column("CTY-TE")]
        public string City { get; set; }
        [Column("CRT-DT")]
        public DateTime CreatedDate { get; set; }
        [Column("STA-TE")]
        public bool State { get; set; }
    }
}
