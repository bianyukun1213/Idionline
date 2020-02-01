using Idionline.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        readonly DataAccess data;
        public EditorController(DataAccess d, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            data = d;
        }
        [HttpGet("login/{platTag}/{code}")]
        public async Task<ActionResult<string>> Login(string platTag, string code)
        {
            HttpClient client = _clientFactory.CreateClient();
            var res = platTag switch
            {
                "WeChat" => await client.GetStringAsync("https://api.weixin.qq.com/sns/jscode2session?appid=wx70c0fb94c40e2986&secret=2160433a6817810b3fc6c3c7731467b9&js_code=" + code + "&grant_type=authorization_code"),
                "QQ" => await client.GetStringAsync("https://api.q.qq.com/sns/jscode2session?appid=1109616357&secret=koKGigxqQK2HzRJe&js_code=" + code + "&grant_type=authorization_code"),
                "QB" => null,
                _ => null,
            };
            client.Dispose();
            return res;
        }
        [HttpPost("register")]
        public async Task<ActionResult<string>> ResgisterEdi([FromBody]EditorRegistrationData ediDt)
        {
            HttpClient client = _clientFactory.CreateClient();
            string platTag = ediDt.PlatTag;
            var res = platTag switch
            {
                "WeChat" => await client.GetStringAsync("https://api.weixin.qq.com/sns/jscode2session?appid=wx70c0fb94c40e2986&secret=2160433a6817810b3fc6c3c7731467b9&js_code=" + ediDt.Code + "&grant_type=authorization_code"),
                "QQ" => await client.GetStringAsync("https://api.q.qq.com/sns/jscode2session?appid=1109616357&secret=koKGigxqQK2HzRJe&js_code=" + ediDt.Code + "&grant_type=authorization_code"),
                "QB" => null,
                _ => null,
            };
            client.Dispose();
            try
            {
                JObject json = JObject.Parse(res);
                string openId = json["openid"].ToString();
                string rtn = data.RegisterEdi(ediDt.NickName, ediDt.PlatTag + "_" + openId);
                return rtn;
            }
            catch (System.Exception)
            {
                return "发生异常，注册失败！";
            }
        }
    }
}
