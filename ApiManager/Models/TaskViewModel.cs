using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ApiManager.Models
{
    public class TaskViewModel
    {
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public Guid SchedulerId { get; set; }

        public ScheduleViewModel Schedule { get; set; }

        public IList<SelectListItem>? Schedulers { get; set; }
    }
}
