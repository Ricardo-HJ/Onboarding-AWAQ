using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Onboarding_AWAQ.Pages
{
    public class EstudioModel : PageModel
    {
        public bool admin { get; set; }
        public string src { get; set; }
        public void OnGet()
            {
                admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
                src = HttpContext.Session.GetString("src");
            }
    }
}
