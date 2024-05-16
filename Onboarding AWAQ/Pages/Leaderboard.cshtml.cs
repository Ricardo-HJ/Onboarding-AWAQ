using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;


namespace Onboarding_AWAQ.Pages
{
    public class LeaderboardModel : PageModel
    {
        public bool admin { get; set; }
        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) == true)
            {
                Response.Redirect("index");
            };
            admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
        }
    }
}
