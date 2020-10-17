using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Qaqa teleb olunur eee bu"),MaxLength(50,ErrorMessage ="Heddivi ashma")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Qaqa teleb olunur eee bu"),StringLength(500,ErrorMessage ="Bes elemedi???")]
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
