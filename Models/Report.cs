using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvc_webapp.Models
{
    public partial class Report
    {
        public int Id { get; set; }
        [DisplayName("REF_NO")]
        public string RefNo { get; set; }

        [DisplayName("FLM_SLM")]
        public string FlmSlm { get; set; }
        
        [DisplayName("MACH_ID")]
        public string MachId { get; set; }
        
        [DisplayName("SERIAL_NO")]
        public string SerialNo { get; set; }
        
        [DisplayName("LOCATION")]
        public string Location { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("DATE")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [DisplayName("ARR_TIME")]
        public TimeSpan? ArrivalTime { get; set; }
        
        [DisplayName("PROB_CODE")]
        public string ProblemCode { get; set; }
        
        [DisplayName("DESC")]
        public string Description { get; set; }
    }

    public class JqueryDatatableParam
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
