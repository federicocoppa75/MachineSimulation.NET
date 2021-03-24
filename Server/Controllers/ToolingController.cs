using Machine.Data;
using Machine.Data.Toolings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolingController : ControllerBase
    {
        private readonly ILogger<ToolingController> _logger;
        private readonly ToolingContext _db;
            
        public ToolingController(ILogger<ToolingController> logger, ToolingContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: api/<ToolingController>
        [HttpGet]
        public IEnumerable<Tooling> Get()
        {
            return _db.Toolings.ToList();
        }

        // GET api/<ToolingController>/5
        [HttpGet("{id}")]
        public Tooling Get(int id)
        {
            return _db.Toolings.FirstOrDefault(o => o.ToolingID == id);
        }

        // POST api/<ToolingController>
        [HttpPost]
        public void Post([FromBody] Tooling tooling)
        {
            _db.Toolings.Add(tooling);
            _db.SaveChanges();
        }

        // PUT api/<ToolingController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ToolingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var t = _db.Toolings.FirstOrDefault(o => o.ToolingID == id);

            if(t != null)
            {
                _db.ToolingUnits.RemoveRange(t.Units);
                _db.Toolings.Remove(t);
                _db.SaveChanges();
            }
        }
    }
}
