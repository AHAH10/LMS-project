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

        [Display(Name ="Published date")]
        public DateTime PublishingDate { get; set; }

        [Display(Name = "Edited date")]
        public DateTime? EditedDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 20)]
        public string Content { get; set; }

        public bool AnonymousPublisher { get; set; }

        [Required]
        [ForeignKey("Publisher")]
        public string PublisherID { get; set; }
        public virtual User Publisher { get; set; }

        [ForeignKey("EditedBy")]
        public string EditedByID { get; set; }
        public virtual User EditedBy { get; set; }


    }
}