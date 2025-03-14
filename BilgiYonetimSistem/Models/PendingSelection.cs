﻿namespace BilgiYonetimSistem.Models
{
    public class PendingSelection
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime SelectedAt { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }
    }
}
