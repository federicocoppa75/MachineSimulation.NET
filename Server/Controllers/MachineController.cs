using Machine.Data;
using Machine.Data.MachineElements;
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
    public class MachineController : ControllerBase
    {
        private readonly ILogger<MachineController> _logger;
        private readonly MachineContext _db;

        public MachineController(ILogger<MachineController> logger, MachineContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: api/<MachineController>
        [HttpGet]
        public IEnumerable<MachineInfo> Get()
        {
            return _db.RootElements.Select(e => new MachineInfo() { MachineElementID = e.MachineElementID, Name = e.AssemblyName })
                                   .ToList();
        }

        // GET api/<MachineController>/5
        [HttpGet("{id}")]
        public MachineElement Get(int id)
        {
            return _db.MachineElements.FirstOrDefault(e => e.MachineElementID == id);
        }

        //// POST api/<MachineController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// POST api/<MachineController>
        //[HttpPost]
        //public void Post([FromBody] MachineElement value)
        //{
        //}

        //// PUT api/<MachineController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        [ProducesResponseType(typeof(int), 200)]
        [HttpPost, HttpPut/*, Route("machine")*/]
        public async Task<IActionResult> Add(RootElement element)
        {
            _db.MachineElements.Add(element);
            await _db.SaveChangesAsync();
            return Ok(element.MachineElementID);
        }

        // DELETE api/<MachineController>/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _db.RemoveElement(id);
        }
    }
}
