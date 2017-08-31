using hiload.Model;
using Newtonsoft.Json.Linq;

namespace hiload.Controllers
{
    public class VisitsController : BaseController<Visit>
    {
        public VisitsController(HiloadContext context): base(context)
        {
        }

        protected override bool Validate(JObject value)
        {
            return ValidInt(value, "location")
                && ValidInt(value, "mark")
                && ValidInt(value, "user")
                && ValidInt(value, "visited_at");
        }
        
        protected override bool Update(Visit entity, JObject value)
        {
            return SetVal(value, "location", v => entity.location = v)
                && SetVal(value, "mark",      v => entity.mark = v)
                && SetVal(value, "user", v => entity.user = v)
                && SetVal(value, "visited_at",     v => entity.visited_at = v);
        }

    }
}
