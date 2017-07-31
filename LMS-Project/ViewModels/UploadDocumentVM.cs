using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS_Project.ViewModels
{
    public class UploadDocumentVM
    {
        public HttpPostedFileBase File { get; set; }
        public int CourseID { get; set; }

        public string DocumentName
        {
            get
            {
                if (File != null)
                    return File.FileName;
                else
                    return String.Empty;
            }
            private set { }
        }

    }
}