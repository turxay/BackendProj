using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [Required,StringLength(100)]
        public string Title { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<SaleProduct> SaleProducts { get; set; }
    }
}
