using Mesh.Data;
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
    public class ModelsController : ControllerBase
    {
        private readonly ILogger<ModelsController> _logger;
        private readonly MeshContext _db;

        public ModelsController(ILogger<ModelsController> logger, MeshContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: api/<ModelsController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet]
        public IEnumerable<MeshInfo> Get()
        {
            return _db.GetMeshInfos();
        }

        // GET api/<ModelsController>/5
        [HttpGet("{id}")]
        public Mesh.Data.Mesh Get(int id)
        {
            return _db.GetMesh(id);
        }

        //// POST api/<ModelsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ModelsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        [ProducesResponseType(typeof(MeshInfo), 200)]
        [HttpPost, HttpPut/*, Route("models")*/]
        public async Task<IActionResult> Add(Mesh.Data.Mesh model/*, bool update = true*/)
        {
            var info = await _db.AddOrUpdateAsync(model, false);
            return Ok(info);
        }

        // DELETE api/<ModelsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _db.Delete(id);
        }
    }
}
