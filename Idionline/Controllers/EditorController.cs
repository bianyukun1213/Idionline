using Idionline.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Localization;
using Idionline.Resources;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly DataAccess _data;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public EditorController(DataAccess d, IStringLocalizer<SharedResource> localizer)
        {
            _data = d;
            _localizer = localizer;
        }
        [HttpPost("login")]
        public StandardReturn Login(/*string platTag, string code*/[FromBody] EditorRegistrationData ediDt)
        {
            try
            {
                return new StandardReturn(result: _data.Login(ediDt.Username, ediDt.Password), localizer: _localizer);

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
        [HttpPost("register")]
        public StandardReturn ResgisterEdi([FromBody] EditorRegistrationData ediDt)
        {
            try
            {
                _data.RegisterEdi(ediDt.Username, ediDt.Password/*, openId, platTag*/);
                return new StandardReturn(/*result: */localizer: _localizer);
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