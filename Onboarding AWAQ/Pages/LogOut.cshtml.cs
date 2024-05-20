using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Onboarding_AWAQ.Pages
{
    public class LogOutModel : PageModel
    {
        public void OnGet()
        {
			if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) == true)
			{
				Response.Redirect("index");
			};
			HttpContext.Session.Clear();
            Response.Redirect("Index");
        }
    }
}
