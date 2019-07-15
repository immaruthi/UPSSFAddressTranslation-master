namespace UPS.Quincus.APP.Request
{
    public class SFOrderXMLRequest
    {
        public string XMLMessage { get; set; }
    }


    public class UIOrderRequestBodyData
    {
        public int id { get; set; }
        public int wfL_ID { get; set; }
        public object qqS_TRA_LG_ID { get; set; }
        public object sF_TRA_LG_ID { get; set; }
        public int smT_STA_NR { get; set; }
        public string smT_NR_TE { get; set; }
        public int pcS_QTY_NR { get; set; }
        public string pkG_NR_TE { get; set; }
        public int pkG_WGT_DE { get; set; }
        public int smT_WGT_DE { get; set; }
        public int diM_WGT_DE { get; set; }
        public string wgT_UNT_TE { get; set; }
        public string svL_NR { get; set; }
        public object pY_MT_TE { get; set; }
        public object shP_DT { get; set; }
        public object pK_UP_TM { get; set; }
        public string biL_TYP_TE { get; set; }
        public int smT_VAL_DE { get; set; }
        public string ccY_VAL_TE { get; set; }
        public object fsT_INV_LN_DES_TE { get; set; }
        public string exP_SLC_CD { get; set; }
        public string shP_NR { get; set; }
        public string shP_CPY_NA { get; set; }
        public string shP_ADR_TR_TE { get; set; }
        public string shP_ADR_TE { get; set; }
        public string orG_CTY_TE { get; set; }
        public string orG_PSL_CD { get; set; }
        public string shP_CTC_TE { get; set; }
        public string shP_PH_TE { get; set; }
        public string imP_SLC_TE { get; set; }
        public string imP_NR { get; set; }
        public string rcV_CPY_TE { get; set; }
        public string rcV_ADR_TE { get; set; }
        public string dsT_CTY_TE { get; set; }
        public string dsT_PSL_TE { get; set; }
        public string csG_CTC_TE { get; set; }
        public string pH_NR { get; set; }
        public string iN_FLG_TE { get; set; }
        public string oU_FLG_TE { get; set; }
        public string pyM_MTD { get; set; }
        public object exP_TYP { get; set; }
        public string coD_TE { get; set; }
        public string poD_RTN_SVC { get; set; }

        public string spC_CST_ID_TE { get; set; }

    }

    public class SFCreateOrderServiceRequest
    {
        public string RequestOrderXMLMessage { get; set; }

        public string Checkword { get; set; }

        public string AccessNumber { get; set; }

        public string BaseURI { get; set; }

        public string RequestURI { get; set; }

    }

    public class SFCancelOrderServiceRequest
    {
        public string RequestOrderXMLMessage { get; set; }

        public string Checkword { get; set; }

        public string AccessNumber { get; set; }

        public string BaseURI { get; set; }

        public string RequestURI { get; set; }

    }
}
