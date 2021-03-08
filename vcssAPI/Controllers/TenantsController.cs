using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vcssAPI.Services;
using vcssAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace vcssAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Tenants")]
    public class TenantsController : Controller
    {
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            mdlTenant tenant = TenantService.Retrieve(id);
            if (tenant == null)
                return NotFound($"Tenant with id:{id} not found");
            return Ok(tenant);
        }
    }
}