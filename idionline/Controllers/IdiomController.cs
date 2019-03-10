using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Idionline.Models;
using MongoDB.Bson;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdiomController : ControllerBase
    {
        DataAccess data;
        public IdiomController(DataAccess d)
        {
            data = d;
        }
        //[HttpGet]
        //public string GenerateIdiom()
        //{
        //    return data.GenerateIdiom();
        //}
        //[HttpGet("count")]
        //public ActionResult<long> GetCount()
        //{
        //    return data.GetIdiomsCount();
        //}
        [HttpGet("{id:length(24)}")]
        public ActionResult<Idiom> GetById(string id)
        {
            Idiom rtn = data.GetIdiomById(new ObjectId(id));
            if (rtn != null)
            {
                return rtn;
            }
            return NotFound();
        }
        [HttpPut("{id:length(24)}")]
        public ActionResult<string> UpdateIdiom(string id, [FromBody]UpdateData dt)
        {
            string rtn = data.UpdateIdiom(new ObjectId(id), dt);
            return rtn;
        }
        [HttpDelete("{id:length(24)}")]
        public ActionResult<string> DeleteIdiom(string id,[FromBody]string openId)
        {
            string rtn = data.DeleteIdiom(new ObjectId(id), openId);
            return rtn;
        }
        [HttpGet("search/{str:length(2,12)}")]
        public ActionResult<Dictionary<string, string>> GetListByStr(string str)
        {
            Dictionary<string, string> rtn = data.GetListByStr(str);
            if (rtn.Count > 0)
            {
                return rtn;
            }
            return NotFound();
        }
        [HttpGet("index/{index:length(1)}")]
        public ActionResult<Dictionary<string, string>> GetListByIndex(char index)
        {
            if (char.IsLetter(char.ToUpper(index)))
            {
                Dictionary<string, string> rtn = data.GetListByIndex(char.ToUpper(index));
                if (rtn.Count > 0)
                {
                    return rtn;
                }
            }
            return NotFound();
        }
        [HttpGet("solitaire/{str:length(2,12)}")]
        public ActionResult<string> Solitaire(string str)
        {
            string rtn = data.Solitaire(str);
            if (rtn != null)
            {
                return rtn;
            }
            return NotFound();
        }
    }
}
