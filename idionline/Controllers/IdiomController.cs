using Microsoft.AspNetCore.Mvc;
using System;
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
        [HttpGet("count")]
        public ActionResult<long> GetCount()
        {
            return data.GetIdiomsCount();
        }
        [HttpGet("{id:length(24)}")]
        public ActionResult<Idiom> GetById(string id)
        {
            try
            {
                return data.GetIdiomById(new ObjectId(id));
            }
            catch (Exception)
            {
                return NotFound();
                throw;
            }
        }
        [HttpGet("search/{str:maxlength(12)}")]
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
    }
}
