using System;
using System.Collections.Generic;
using System.Linq;
using hiload.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace hiload.Controllers
{
    public class LocationsController : BaseController<Location>
    {
        public LocationsController(HiloadContext context): base(context)
        {
        }

        protected override bool Validate(JObject value)
        {
            return ValidStr(value, "city")
                && ValidStr(value, "country")
                && ValidInt(value, "distance")
                && ValidStr(value, "place");
        }

        protected override bool Update(Location entity, JObject value)
        {
            var cnt = new List<Action>();

            var rez = SetVal(value, "city",  v => entity.city = v)
                && SetVal(value, "country",  v => entity.country = v)
                && SetVal(value, "distance", v => entity.distance = v)
                && SetVal(value, "place",    v => entity.place = v);

            if (rez) {
                foreach (var item in cnt)
                {
                    item();
                }
            }

            return rez;
        }

        // GET values/5/avg
        [HttpGet("{id}/avg")]
        public IActionResult GetAvg(int id, [FromQuery] GetAvgQuery query)
        {
            var location = Context.Locations.Find(id);
            if (location == null) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            IEnumerable<Visit> fromVisits = location.GetVisits();
            // if (query.gender!=null || query.fromAge!=null || query.toAge!=null){
            //     fromVisits = fromVisits.Include(p => p.User);
            // }

            fromVisits = fromVisits.Where(v => v.location == id);
            
            if (query.fromDate!=null){
                fromVisits = fromVisits.Where(v => v.visited_at > query.fromDate);
            }
            if (query.toDate!=null){
                fromVisits = fromVisits.Where(v => v.visited_at < query.toDate);
            }
            if (query.gender!=null){
                fromVisits = fromVisits.Where(v => v.GetUser().gender == query.gender);
            }
            if (query.fromAge!=null){
                fromVisits = fromVisits.Where(v => v.GetUser().GetAge() >= query.fromAge);
            }
            if (query.toAge!=null){
                fromVisits = fromVisits.Where(v => v.GetUser().GetAge() < query.toAge);
            }

            // if (id == 514){
            //     var tmp = Context.Visits.Where(v => v.location == id);
            //     foreach (var t in tmp)
            //     {
            //         var date = hiload.Model.User.UnixDate.AddSeconds(t.User.birth_date);
            //         Console.WriteLine($"{t.User.id} {t.User.birth_date} {date} {t.User.Age} {t.visited_at} {t.mark}" );
            //     }
            // }

            double avg = Math.Round(fromVisits.DefaultIfEmpty(_default).Average(x=>x.mark), 5);

            return Json(new {avg});
        }

        private static Visit _default = new Visit();
    }

}
