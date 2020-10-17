using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public ICollection<SaleProduct> SaleProducts { get; set; }

    }
}
