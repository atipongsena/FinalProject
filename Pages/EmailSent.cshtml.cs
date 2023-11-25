using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalProject.Pages
{
    [Authorize]
    public class EmailSentModel : PageModel
    {
        // You can add properties or methods if needed for the EmailSent page
        public void OnGet()
        {
            // Any initialization code for the EmailSent page goes here
        }
    }
}