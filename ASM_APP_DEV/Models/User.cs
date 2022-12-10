using Microsoft.AspNetCore.Identity;

namespace ASM_APP_DEV.Models
{
	public class User : IdentityUser
	{
		public string FullName { get; set; }
		public string Address { get; set; }

	}
}
