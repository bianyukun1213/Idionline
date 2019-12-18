using Idionline.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaunchInfoController : ControllerBase
    {
        readonly DataAccess data;
        public LaunchInfoController(DataAccess d)
        {
            data = d;
        }
        //[HttpGet]
        //public string GenerateLaunchInf()
        //{
        //    return data.GenerateLaunchInf();
        //}
        [HttpGet("{date:maxlength(12)}/{openId?}")]
        public ActionResult<LaunchInfo> GetLaunchInf(long date,string openId)
        {
            try
            {
                DateTimeOffset dateUT = DateTimeOffset.FromUnixTimeSeconds(date).ToLocalTime();
                int hour = dateUT.Hour;
                int min = dateUT.Minute;
                int sec = dateUT.Second;
                long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
                return data.GetLaunchInf(dateL,openId);
            }
            catch (Exception)
            {
                return NotFound();
                throw;
            }
        }
    }
}