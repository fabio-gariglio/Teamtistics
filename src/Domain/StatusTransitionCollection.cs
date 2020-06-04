using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class StatusTransitionCollection : IReadOnlyCollection<StatusTransition>
    {
        private readonly IList<StatusTransition> _statusTransitions;

        public StatusTransitionCollection(IEnumerable<StatusTransition> statusTransitions)
        {
            _statusTransitions = statusTransitions.ToList();
        }

        public StatusTransitionCollection() : this(Enumerable.Empty<StatusTransition>())
        {
        }

        public IEnumerator<StatusTransition> GetEnumerator() => _statusTransitions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _statusTransitions.GetEnumerator();

        public int Count => _statusTransitions.Count;

        public override string ToString() =>
            _statusTransitions.Any()
                ? $"{_statusTransitions.First().From}->{string.Join("->", _statusTransitions.Select(s => s.To))}"
                : string.Empty;
    }
}