using mvc_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc_webapp.ViewModels
{
    public class ReportViewModel
    {
        public ReportViewModel(Report report)
        {
            MachId = report.MachId ?? "";
            FlmSlm = report.FlmSlm ?? "";
            Date = report.Date;
            ArrivalTime = report.ArrivalTime.HasValue ? report.ArrivalTime.Value.ToString() : null;
            ProblemCode = report.ProblemCode ?? "";
            Description = report.Description ?? "";
            
        }

        public string MachId { get; set; }
        public string FlmSlm { get; set; }
        public DateTime? Date { get; set; }
        public string ArrivalTime { get; set; }
        public string ProblemCode { get; set; }
        public string Description { get; set; }
    }
}
