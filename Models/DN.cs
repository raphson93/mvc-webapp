using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvc_webapp.Models
{
    public class DN
    {
        [DisplayName("Request")]
        public string Request { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Reported Date")]
        [DataType(DataType.DateTime)]
        public DateTime? ReportedDt { get; set; }
        
        [DisplayName("Tag")]
        public string Tag { get; set; }

        [DisplayName("Serial Number")]
        public string SerialNo { get; set; }

        [DisplayName("Request Type")]
        public string ReqType { get; set; }

        [DisplayName("Problem Summary")]
        public string ProbSumamry { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Work Start")]
        [DataType(DataType.DateTime)]
        public DateTime? WorkStart { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Work End")]
        [DataType(DataType.DateTime)]
        public DateTime? WorkEnd { get; set; }

        [DisplayName("Resolution")]
        public string Resolution { get; set; }

        [DisplayName("Service Coder")]
        public string ServiceCoder { get; set; }

        public class JqueryDNParam
        {
            public string sEcho { get; set; }
            public string sSearch { get; set; }
            public int iDisplayLength { get; set; }
            public int iDisplayStart { get; set; }
            public int iColumns { get; set; }
            public int iSortCol_0 { get; set; }
            public string sSortDir_0 { get; set; }
            public int iSortingCols { get; set; }
            public string sColumns { get; set; }
        }
    }
}
