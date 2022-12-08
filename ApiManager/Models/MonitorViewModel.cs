using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApiManager.Models
{
    public class MonitorViewModel
    {
        public string SchedulerId { get; set; }
        public IList<SelectListItem> Schedulers { get; set; }
    }
}
