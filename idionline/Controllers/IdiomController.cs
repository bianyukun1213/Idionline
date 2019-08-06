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
        readonly DataAccess data;
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
        [HttpGet("{id:length(24)}/{openId?}")]
        public ActionResult<Idiom> GetById(string id,string openId)
        {
            try
            {
                Idiom rtn = data.GetIdiomById(new ObjectId(id),openId);
                return rtn;
            }
            catch (System.Exception)
            {
                return NotFound();
                throw;
            }
        }
        [HttpGet("{id:length(24)}/bson/{openId?}")]
        public ActionResult<string> GetBsonById(string id,string openId)
        {
            try
            {
                Idiom rtn = data.GetIdiomById(new ObjectId(id),openId);
                return rtn.ToBsonDocument().ToString();
            }
            catch (System.Exception)
            {
                return NotFound();
                throw;
            }
        }
        [HttpPost("create/from_juhe")]
        public ActionResult<string> CrtIdiFrmJh([FromBody]JuheIdiomData dt)
        {
            string rtn = data.CreateIdiom(dt);
            return rtn;
        }
        [HttpPut("{id:length(24)}")]
        public ActionResult<string> UpdateIdiom(string id, [FromBody]UpdateData dt)
        {
            try
            {
                string rtn = data.UpdateIdiom(new ObjectId(id), dt);
                return rtn;
            }
            catch (System.Exception)
            {
                return NotFound();
                throw;
            }
        }
        [HttpDelete("{id:length(24)}")]
        public ActionResult<string> DeleteIdiom(string id, [FromBody]string openId)
        {
            try
            {
                string rtn = data.DeleteIdiom(new ObjectId(id), openId);
                return rtn;
            }
            catch (System.Exception)
            {
                return NotFound();
                throw;
            }
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
        [HttpGet("search/id/{id:length(24)}")]
        public ActionResult<Dictionary<string, string>> GetListById(string id)
        {
            try
            {
                Dictionary<string, string> rtn = data.GetListById(new ObjectId(id));
                return rtn;
            }
            catch (System.Exception)
            {
                return NotFound();
                throw;
            }
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
