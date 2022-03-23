using System.ComponentModel;

namespace BulkyBook.Models
{
    public class CoverType
    {
        public int Id { get; set; }

        [DisplayName("Cover Type")]
        public string Name { get; set; }

    }
}
