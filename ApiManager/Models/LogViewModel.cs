using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApiManager.Models
{
    public class LogViewModel
    {
        public string UserId { get; set; }
        public IList<SelectListItem> Users { get; set; }

        public string TaskId { get; set; }
        public IList<SelectListItem> Tasks { get; set; }
    }
}
