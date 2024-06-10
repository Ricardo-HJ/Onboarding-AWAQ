using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class CreditosModel : PageModel
    {
        public bool admin { get; set; }
        public string src { get; set; }
    }
}
