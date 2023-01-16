﻿namespace TaskProcessing.Core.Models
{
    public class DailySchedule : Schedule
    {

        /// <summary>
        /// Sets or retrieves the interval between the days in the schedule.
        /// </summary>
        public int DaysInterval { get; set; }
    }
}
