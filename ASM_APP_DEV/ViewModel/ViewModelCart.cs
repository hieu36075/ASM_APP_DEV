using ASM_APP_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ASM_APP_DEV.ViewModel
{
    public class ViewModelCart
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Order Order { get; set; }
        [BindProperty]
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
