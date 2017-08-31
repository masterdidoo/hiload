using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hiload.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace hiload.Controllers
{
    public class UsersController : BaseController<User>
    {
        public UsersController(HiloadContext context): base(context)
        {
        }

        protected override bool Validate(JObject value)
        {
            return ValidInt(value, "birth_date")
                && ValidStr(value, "email")
                && ValidStr(value, "first_name")
                && ValidStr(value, "gender")
                && ValidStr(value, "last_name");
        }

        protected override bool Update(User entity, JObject value)
        {
            return SetVal(value, "birth_date", v => entity.birth_date = v)
                && SetVal(value, "email",      v => entity.email = v)
                && SetVal(value, "first_name", v => entity.first_name = v)
                && SetVal(value, "gender",     v => entity.gender = v)
                && SetVal(value, "last_name",  v => entity.last_name = v);
        }

        // GET values/5/visits
        [HttpGet("{id}/visits")]
        public IActionResult GetVisits(int id, [FromQuery] GetVisitsQuery query)
        {
            var user = Context.Users.Find(id);
            if (user == null) return NotFound();

            if (!ModelState.IsValid) return BadRequest();

            IEnumerable<Visit> fromVisits = user.GetVisits();

            if (!ModelState.IsValid) return BadRequest();

            if (query.fromDate!=null){
                fromVisits = fromVisits.Where(v => v.visited_at > query.fromDate);
            }
            if (query.toDate!=null){
                fromVisits = fromVisits.Where(v => v.visited_at < query.toDate);
            }
            if (query.toDistance!=null){
                fromVisits = fromVisits.Where(v => v.GetLocation().distance < query.toDistance);
            }
            if (query.country!=null){
                fromVisits = fromVisits.Where(v => v.GetLocation().country == query.country);
            }

            // var visits = fromVisits.OrderBy(v=>v.visited_at).ToArray().Select(v=>new {
            var visits = fromVisits.Select(v=>new {
                mark = v.mark,
                visited_at = v.visited_at,
                place = v.GetLocation().place
            });

            return Json(new {
                visits = visits
            });
        }
    }
}
