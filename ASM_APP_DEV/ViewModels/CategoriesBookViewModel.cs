using ASM_APP_DEV.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASM_APP_DEV.ViewModels
{
    public class CategoriesBookViewModel
    {
        public Book Book { get; set; }
        [Required]
        public int BrandId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
