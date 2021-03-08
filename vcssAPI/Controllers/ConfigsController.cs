using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vcssAPI.Models;
using vcssAPI.Services;
using vcssAPI.DBContext;
using Microsoft.Extensions.Options;

namespace vcssAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Configs")]
    public class ConfigsController : Controller
    {        
        [HttpGet]
        public IEnumerable<mdlConfig> Get()
        {
            return ConfigService.Retrieve();            
        }

        [HttpGet("{id}")]
        public IEnumerable<mdlConfig> Get(int id)
        {
            return ConfigService.Retrieve(id);            
        }

        [HttpPost]
        public ActionResult Post([FromBody] mdlConfig config)
        {
            return Ok(ConfigService.Create(config));
        }
        
        [HttpPut]
        public ActionResult Put([FromBody] mdlConfig config)
        {
            return Ok(ConfigService.Update(config));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok(ConfigService.Delete(id));
        }
    }
}