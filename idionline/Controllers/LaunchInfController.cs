using Idionline.Models;
using Microsoft.AspNetCore.Mvc;
using System;

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
        [HttpGet("{date:maxlength(12)}")]
        public ActionResult<LaunchInf> GetLaunchInf(long date)
        {
            try
            {
                return data.GetLaunchInf(date);
            }
            catch (Exception)
            {
                return NotFound();
                throw;
            }
        }
    }
}