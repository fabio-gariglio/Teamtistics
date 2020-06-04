using System;

namespace Domain
{
    public class StatusTransition
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public override string ToString() => $"{From}->{To}";
    }
}