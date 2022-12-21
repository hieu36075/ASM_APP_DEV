﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ASM_APP_DEV.Models
{
	public class User : IdentityUser
	{
		public string FullName { get; set; }
		public string Address { get; set; }


		public List<Order> Orders { get; set; }

	}
}
