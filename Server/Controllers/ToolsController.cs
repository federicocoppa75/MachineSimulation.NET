using Machine.Data;
using Machine.Data.Tools;
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
    public class ToolsController : ControllerBase
    {
        private readonly ILogger<ToolsController> _logger;
        private readonly ToolsContext _db;

        public ToolsController(ILogger<ToolsController> logger, ToolsContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: api/<ToolsController>
        [HttpGet]
        public IEnumerable<ToolsetInfo> Get()
        {
            return _db.ToolSets.Select(e => new ToolsetInfo() { Name = e.Name, ToolSetID = e.ToolSetID })
                               .ToList();
        }

        // GET api/<ToolsController>/5
        [HttpGet("{id}")]
        public ToolSet Get(int id)
        {
            return _db.ToolSets.FirstOrDefault(e => e.ToolSetID == id);
        }

        // POST api/<ToolsController>
        [HttpPost]
        public void Post([FromBody] ToolSet toolSet)
        {
            _db.ToolSets.Add(toolSet);
            _db.SaveChanges();
        }

        // PUT api/<ToolsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Tool tool)
        {
        }

        // DELETE api/<ToolsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var ele = _db.ToolSets.FirstOrDefault(e => e.ToolSetID == id);
            
            if(ele != null)
            {
                _db.Tools.RemoveRange(ele.Tools);
                _db.ToolSets.Remove(ele);
                _db.SaveChanges();
            }
        }
    }
}
