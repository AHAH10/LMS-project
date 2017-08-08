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

        [Required]
        public string DocumentName { get; set; }

        [Required]
        public byte[] DocumentContent { get; set; }

        public string ContentType { get; set; }

        [Required]
        public DateTime UploadingDate { get; set; }

        [Required]
        [ForeignKey("Uploader")]
        public string UploaderID { get; set; }
        public virtual User Uploader { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        [ForeignKey("VisibleFor")]
        public string RoleID { get; set; }
        public virtual Role VisibleFor { get; set; }
    }
}