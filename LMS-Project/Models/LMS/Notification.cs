using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Project.Models.LMS
{
    public class Notification
    {
        [Key]
        [ForeignKey("Grade")]
        public int ID { get; set; }

        public virtual Grade Grade { get; set; }

        public DateTime SendingDate { get; set; }
        public DateTime? ReadingDate { get; set; }
    }
}