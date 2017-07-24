using System.ComponentModel.DataAnnotations;

namespace Models.LMS
{
    public class Classroom
    {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string Location { get; set; }
    }
}
