using ProATA.SharedKernel.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiManager.Models
{
    public class ScheduleViewModel
    {
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        public ScheduleType Type { get; set; }

        [Display(Name = "Start")]
        public string StartBoundery { get; set; }

        [Display(Name = "End")]
        public string EndBoundery { get; set; }

        public short Interval { get; set; }

        public bool Repeat { get; set; }

        public int RepeatAmount { get; set; }

        public Unit RepeatUnit { get; set; }

        public bool Enabled { get; set; }
        public IList<int> Days { get; set; }

        public string CronExpression { get; set; }
    }

}
