using Microsoft.AspNetCore.Mvc;
using Idionline.Models;
using System;
using Microsoft.Extensions.Localization;
using Idionline.Resources;
using System.Text.RegularExpressions;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdiomController : ControllerBase
    {
        private readonly DataAccess _data;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public IdiomController(DataAccess d, IStringLocalizer<SharedResource> localizer)
        {
            _data = d;
            _localizer = localizer;
        }
        [HttpGet("{id:length(24)}")]
        public StandardReturn GetById(string id, [FromHeader] string cookie, int bson)
        {
            try
            {
                string sessionId = null;
                if (!string.IsNullOrEmpty(cookie))
                {
                    var match = Regex.Match(cookie, "SESSIONID=(.+?);");
                    sessionId = match.Success ? match.Value.Replace("SESSIONID=", "").Replace(";", "") : null;
                }
                return new StandardReturn(result: _data.GetIdiomById(id, sessionId, bson), localizer: _localizer);
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
        [HttpGet("test")]
        public string Test()
        {
            return _data.Test();
        }
        [HttpPut("{id:length(24)}")]
        public StandardReturn UpdateIdiom(string id, [FromBody] UpdateData dt, [FromHeader] string cookie)
        {
            try
            {
                string sessionId = null;
                if (!string.IsNullOrEmpty(cookie))
                {
                    var match = Regex.Match(cookie, "SESSIONID=(.+?);");
                    sessionId = match.Success ? match.Value.Replace("SESSIONID=", "").Replace(";", "") : null;
                }
                _data.UpdateIdiom(id, sessionId, dt);
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
        [HttpDelete("{id:length(24)}")]
        public StandardReturn DeleteIdiom(string id, [FromHeader] string cookie)
        {
            try
            {
                string sessionId = null;
                if (!string.IsNullOrEmpty(cookie))
                {
                    var match = Regex.Match(cookie, "SESSIONID=(.+?);");
                    sessionId = match.Success ? match.Value.Replace("SESSIONID=", "").Replace(";", "") : null;
                }
                _data.DeleteIdiom(id, sessionId);
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




        [HttpPost("advanced-search")]
        public StandardReturn AdvanceSearch([FromBody] AdvancedSearchData data, [FromHeader] string cookie)
        {
            try
            {
                string sessionId = null;
                if (!string.IsNullOrEmpty(cookie))
                {
                    var match = Regex.Match(cookie, "SESSIONID=(.+?);");
                    sessionId = match.Success ? match.Value.Replace("SESSIONID=", "").Replace(";", "") : null;
                }
                return new StandardReturn(result: _data.AdvancedSearch(sessionId, data.LambdaExpression), localizer: _localizer);
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




        [HttpGet("search/{str:length(2,12)}")]
        public StandardReturn GetListByStr(string str)
        {
            try
            {
                return new StandardReturn(result: _data.GetListByStr(str), localizer: _localizer);
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
        [HttpGet("search/{id:length(24)}")]
        public StandardReturn GetListById(string id)
        {
            try
            {
                return new StandardReturn(result: _data.GetListById(id), localizer: _localizer);
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
        [HttpGet("search/{index:length(1)}")]
        public StandardReturn GetListByIndex(char index)
        {
            try
            {
                return new StandardReturn(result: _data.GetListByIndex(char.ToUpper(index)), localizer: _localizer);
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
        [HttpGet("playsolitaire/{str:length(2,12)}")]
        public StandardReturn PlaySolitaire(string str)
        {
            try
            {
                return new StandardReturn(result: _data.PlaySolitaire(str), localizer: _localizer);
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