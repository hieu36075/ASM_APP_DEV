using System.Collections.Generic;

namespace ASM_APP_DEV.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdBook { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public Order Order { get; set; } 
        public Book Book { get; set; }   
    }
}
