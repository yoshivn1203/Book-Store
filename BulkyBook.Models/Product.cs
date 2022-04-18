using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "List Price")]

        [Range(1, 10000)]
        public double ListPrice { get; set; }
        [Display(Name = "Price for 1-50")]

        [Range(1, 10000)]
        public double Price { get; set; }
        [Display(Name = "Price for 51-100")]

        [Range(1, 10000)]
        public double Price50 { get; set; }
        [Display(Name = "Price for 100+")]

        [Range(1, 10000)]
        public double Price100 { get; set; }
        [ValidateNever]

        [Display(Name = "Upload Product Image")]
        public string ImageUrl { get; set; }
        [Display(Name = "Category")]

        public int CategoryID { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [Display(Name ="Cover Type")]
        public int CoverTypeID { get; set; }
        [ValidateNever]
        public CoverType  CoverType { get; set; }

        public string ShortenDescription(int charCount)
        {
            if(Description.Length > charCount)
            {
                while (!char.IsWhiteSpace(Description[charCount]))
                {
                    charCount++;
                }
                string shortenDescription = Description.Substring(0, charCount) + "...";
                return shortenDescription;
            }
            return Description;
        }
    }
}
