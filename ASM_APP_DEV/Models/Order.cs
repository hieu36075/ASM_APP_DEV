using ASM_APP_DEV.Enums;
using System;
using System.Collections.Generic;

namespace ASM_APP_DEV.Models
{
    public class Order 
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateOrder { get; set; } = DateTime.Now;
        public int PriceOrder { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public User User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
