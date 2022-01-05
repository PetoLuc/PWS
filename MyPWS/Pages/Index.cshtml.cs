using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPWS.Models.pwsstore;
using System.Linq;

namespace MyPWS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PwsStoreContext _pwsStoreContext;

        public IndexModel(PwsStoreContext pwsStoreContext)
        {
            _pwsStoreContext = pwsStoreContext;
        }

        public Weather LastWeather => this._pwsStoreContext.Weather.OrderBy(w=>w.Id).LastOrDefault();

        public void OnGet()
        {
        }
        
    }
}
