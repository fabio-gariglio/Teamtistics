using System.Collections.Generic;

namespace Domain
{
    public class Item
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string User { get; set; }
        public string Sprint { get; set; }
        public Estimate Estimate { get; set; }
        public StatusTransitionCollection Transitions { get; set; } = new StatusTransitionCollection();
        public string Status { get; set; }

        public override string ToString() => $"[{Id}] {Title}";
    }
}
