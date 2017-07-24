using System.ComponentModel.DataAnnotations;

namespace Models.LMS
{
    public class Subject
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
