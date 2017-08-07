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
        public int ID { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public AssignmentGrade AGrade { get; set; }
        [Required]
        public string Comment { get; set; }
        [ForeignKey("Document")]
        public virtual int DocumentID { get; set; }
        public virtual Document Document { get; set; }
    }
}