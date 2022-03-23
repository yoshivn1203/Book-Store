using System.ComponentModel;

namespace BulkyBook.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

    }
}
