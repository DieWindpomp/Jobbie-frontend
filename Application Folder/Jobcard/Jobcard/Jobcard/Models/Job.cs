using System;
using System.Collections.Generic;
using System.Text;

namespace Jobcard.Models
{
    public class Job
    {
        public string JobID { get; set; }
        public string JobDescription { get; set; }
        public string LocationID { get; set; }
        public DateTime Date {get;set;}
        public string Urgency { get; set; }
        public bool Active { get; set; }
        public string Comment { get; set; }
        public bool Complete { get; set; }
       

        public Job(string JobID, string JobDescription, string LocationID, DateTime Date,string Urgency, bool Active, string Comment, bool Complete)
        {
            this.JobID = JobID;
            this.JobDescription = JobDescription;
            this.LocationID = LocationID;
            this.Date = Date;
            this.Urgency = Urgency;
            this.Active = Active;
            this.Comment = Comment;
            this.Complete = Complete;
        }

    }
}
