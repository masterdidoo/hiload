using System.Collections.Generic;

namespace hiload.Model
{
    public class VisitConnected
    {
        private readonly ListNode<Visit> visit = new ListNode<Visit>(null);

        internal IEnumerable<Visit> GetVisits()
        {
            return visit.GetAll();
        }

        internal void AddVisit(ListNode<Visit> locationVisit)
        {
            var target = visit;
            var next = visit.Next;

            while(next != null && next.Value.visited_at < locationVisit.Value.visited_at) {
                target = next;
                next = next.Next;
            }

            locationVisit.Add(target);
        }
    }    
}