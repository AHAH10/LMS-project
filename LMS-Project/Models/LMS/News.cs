using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS_Project.Models.LMS
{
    public class News
    {
        [Key]
        public int ID { get; set; }
        public DateTime PublishingDate { get; set; }
        public string Title { get; set; }
        public string NewsContent { get; set; }

        [ForeignKey("Uploader")]
        public int PublisherID { get; set; }
        public virtual User Uploader { get; set; }
    }
}