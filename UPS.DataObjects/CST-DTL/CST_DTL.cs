namespace UPS.DataObjects.CST_DTL
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CST_DTL
    {
        [Key]
        [Column("ID")]
        public int? ID { get; set; }
        [Column("CST-LNK-ID")]
        public int? CST_LNK_ID { get; set; }
        [Column("CST-ID")]
        public string CST_ID { get; set; }
        [Column("CST-CPY-ONE-TE")]
        public string CST_CPY_ONE_TE { get; set; }
        [Column("CST-CPY-TWO-TE")]
        public string CST_CPY_TWO_TE { get; set; }
        [Column("CST-ADR-TE")]
        public string CST_ADR_TE { get; set; }
        [Column("CST-DST-TE")]
        public string CST_DST_TE { get; set; }
    }
}
