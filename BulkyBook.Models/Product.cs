using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        [Range(1, 10000)]
        public double ListPrice { get; set; }

        [Range(1, 10000)]
        public double Price { get; set; }

        [Range(1, 10000)]
        public double Price50 { get; set; }

        [Range(1, 10000)]
        public double Price100 { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public int CoverTypeID { get; set; }

        public CoverType  CoverType { get; set; }
    }
}
