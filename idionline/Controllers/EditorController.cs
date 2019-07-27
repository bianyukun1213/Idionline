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
        DataAccess data;
        public EditorController(DataAccess d, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            data = d;
        }
        [HttpGet("login/{platStr}/{code}")]
        public async Task<ActionResult<string>> Login(string platStr, string code)
        {
            HttpClient client = _clientFactory.CreateClient();
            string res;
            switch (platStr)
            {
                case "WeChat":
                    res = await client.GetStringAsync("https://api.weixin.qq.com/sns/jscode2session?appid=wx70c0fb94c40e2986&secret=2160433a6817810b3fc6c3c7731467b9&js_code=" + code + "&grant_type=authorization_code");
                    break;
                case "QQ":
                    res = await client.GetStringAsync("https://api.q.qq.com/sns/jscode2session?appid=1109616357&secret=koKGigxqQK2HzRJe&js_code=" + code + "&grant_type=authorization_code");
                    break;
                case "QB":
                    res = null;
                    break;
                default:
                    res = null;
                    break;
            }
            return res;
        }
        //[HttpPost("register")]
        //public ActionResult<string> ResgisterEdi([FromBody]EditorRegisterData ediDt)
        //{
        //    string rtn = data.RegisterEdi(ediDt);
        //    return rtn;
        //}
        [HttpPost("register")]
        public async Task<ActionResult<string>> ResgisterEdi([FromBody]EditorRegisterData ediDt)
        {
            HttpClient client = _clientFactory.CreateClient();
            string platStr = ediDt.PlatStr;
            string res;
            switch (platStr)
            {
                case "WeChat":
                    res = await client.GetStringAsync("https://api.weixin.qq.com/sns/jscode2session?appid=wx70c0fb94c40e2986&secret=2160433a6817810b3fc6c3c7731467b9&js_code=" + ediDt.Code + "&grant_type=authorization_code");
                    break;
                case "QQ":
                    res = await client.GetStringAsync("https://api.q.qq.com/sns/jscode2session?appid=1109616357&secret=koKGigxqQK2HzRJe&js_code=" + ediDt.Code + "&grant_type=authorization_code");
                    break;
                case "QB":
                    res = null;
                    break;
                default:
                    res = null;
                    break;
            }
            try
            {
                JObject json = JObject.Parse(res);
                string openId = json["openid"].ToString();
                string rtn = data.RegisterEdi(ediDt.NickName, ediDt.PlatStr + "_" + openId);
                return rtn;
            }
            catch (System.Exception)
            {
                return "发生异常，注册失败！";
            }
        }
    }
}
