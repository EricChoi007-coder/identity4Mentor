using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize("ApiScope")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        public IActionResult Get()
        {
            return new JsonResult(from claim in User.Claims select new
            {
                claim.Type, claim.Value
            });
        }
    }
}
