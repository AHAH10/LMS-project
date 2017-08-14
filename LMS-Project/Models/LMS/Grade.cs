using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Project.Models.LMS
{
    public enum AssignmentGrade
    {
        A,
        B,
        C,
        D,
        E,
        F
    }
    public class Grade
    {
        [Key]
        [ForeignKey("Document")]
        public int ID { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [Display(Name="Grade")]
        public AssignmentGrade AGrade { get; set; }
        
        public string Comment { get; set; }

        public virtual Document Document { get; set; }

        [ForeignKey("Notification")]
        public int? NotificationID { get; set; }
        public virtual Notification Notification { get; set; }
    }
}