using Idionline.Models;
using Idionline.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaunchInfoController : ControllerBase
    {
        private readonly DataAccess _data;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public LaunchInfoController(DataAccess d, IStringLocalizer<SharedResource> localizer)
        {
            _data = d;
            _localizer = localizer;
        }
        [HttpGet("{date:maxlength(12)}")]
        public StandardReturn GetLaunchInfo(long date, [FromHeader] string sessionId)
        {
            try
            {
                DateTimeOffset dateUT = DateTimeOffset.FromUnixTimeSeconds(date).ToLocalTime();
                int hour = dateUT.Hour;
                int min = dateUT.Minute;
                int sec = dateUT.Second;
                long dateL = dateUT.AddSeconds(-sec).AddMinutes(-min).AddHours(-hour).ToUnixTimeSeconds();
                return new StandardReturn(result: _data.GetLaunchInfo(dateL, sessionId), localizer: _localizer);
            }
            catch (Exception e)
            {
                if (e is EasyException)
                    return new StandardReturn(code: (e as EasyException).ErrorCode, localizer: _localizer);
                else
                    return new StandardReturn(code: -1, localizer: _localizer);
                //throw;
            }
        }
    }
}