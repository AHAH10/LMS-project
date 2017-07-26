using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace LMS_Project.Models.LMS
{
    public class Document
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Uploader")]
        public string UserID { get; set; }
        public virtual User Uploader { get; set; }
        public string DocumentName { get; set; }
        public string DocumentContent { get; set; }
        public DateTime UploadingDate { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        [ForeignKey("VisibleTo")]
        public string RoleID { get; set; }
        public virtual Role VisibleTo { get; set; }
    }
}