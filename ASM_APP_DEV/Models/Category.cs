using ASM_APP_DEV.Enums;
using System.Collections.Generic;

namespace ASM_APP_DEV.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string NameCategory { get; set; }
        public CategoryStatus CategoryStatus { get; set; }

        public List<Book> Books { get; set; }
    }
}
