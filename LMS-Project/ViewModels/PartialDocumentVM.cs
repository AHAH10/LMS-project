using LMS_Project.Models.LMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS_Project.ViewModels
{
    public class PartialDocumentVM
    {
        public int ID { get; set; }

        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Display(Name = "Uploaded")]
        public DateTime UploadingDate { get; set; }

        [Display(Name = "By")]
        public string Uploader { get; set; }

        public string RoleID { get; set; }

        public Grade Grade { get; set; }

        public bool CanSetAGrade { get; set; }
        public bool CanBeDeleted { get; set; }
    }
}