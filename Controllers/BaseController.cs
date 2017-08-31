using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hiload.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace hiload.Controllers
{
    [Route("[controller]")]
    public abstract class BaseController<T> : Controller where T: class, IEntity, new()
    {
        protected readonly HiloadContext Context;
        private static object _empty = new {};
        private static JsonResult _emptyOk = new JsonResult(_empty);

        protected BaseController(HiloadContext context)
        {
            Context = context;
        }

        // GET values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = Context.Set<T>().Find(id);

            if (entity == null) return NotFound();

            return Json(entity);
        }

        // POST values/new
        [HttpPost("new")]
        public IActionResult Insert([FromBody]T value)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                if (Context.Set<T>().Find(value.id) != null) return BadRequest(_empty);
                Context.Set<T>().Add(value);
            }
            catch 
            {
                return BadRequest();
            }

            return _emptyOk;
        }

        // POST values/5
        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody]JObject value)
        {
            if (value == null || !Validate(value)) return BadRequest();

            var entity = Context.Set<T>().Find(id);
            if (entity == null) return NotFound();

            if (!Update(entity, value)) return BadRequest();

            return _emptyOk;
        }

        protected abstract bool Validate(JObject value);

        protected abstract bool Update(T entity, JObject value);

        protected bool SetVal(JObject value, string valName, Action<string> setter) {
            return SetVal(value, valName, JTokenType.String, (v) => setter(v.Value<string>()));
        }

        protected bool SetVal(JObject value, string valName, Action<int> setter) {
            return SetVal(value, valName, JTokenType.Integer, (v) => setter(v.Value<int>()));
        }

        private bool SetVal(JObject value, string valName, JTokenType type, Action<JToken> p)
        {
            var val = value.GetValue(valName);
            if (val != null) {
                p(val);
            }
            return true;
        }

        protected bool ValidStr(JObject value, string valName) {
            return ValidVar(value, valName, JTokenType.String);
        }

        protected bool ValidInt(JObject value, string valName) {
            return ValidVar(value, valName, JTokenType.Integer);
        }

        private bool ValidVar(JObject value, string valName, JTokenType type)
        {
            var val = value.GetValue(valName);
            if (val != null) {
                if (val.Type != type) return false;
            }
            return true;
        }
    }
}
