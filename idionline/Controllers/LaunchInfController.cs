using System.Collections.Generic;
using Idionline.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaunchInfController : ControllerBase
    {
        DataAccess data;
        public LaunchInfController(DataAccess d)
        {
            data = d;
        }
        //[HttpGet]
        //public string GenerateLaunchInf()
        //{
        //    return data.GenerateLaunchInf();
        //}
        [HttpGet("{date}")]
        public ActionResult<List<LaunchInf>> GetLaunchInf(long date)
        {
            List<LaunchInf> rtn = data.GetLaunchInf(date);
            if (rtn.Count > 0)
            {
                return rtn;
            }
            return NotFound();
        }
    }
}
