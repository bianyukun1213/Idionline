using Microsoft.AspNetCore.Mvc;
using Idionline.Models;

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
        [HttpGet("{id:length(24)}")]
        public StandardReturn GetById(string id, string openId, int bson)
        {
            try
            {
                return data.GetIdiomById(id, openId, bson);
            }
            catch (System.Exception)
            {
                return new StandardReturn(-1);
                throw;
            }
        }
        //[HttpGet("test")]
        //public void Test()
        //{
        //    data.Test();
        //}
        [HttpPut("{id:length(24)}")]
        public StandardReturn UpdateIdiom(string id, [FromBody] UpdateData dt)
        {
            try
            {
                return data.UpdateIdiom(id, dt);
            }
            catch (System.Exception)
            {
                return new StandardReturn(-1);
                throw;
            }
        }
        [HttpDelete("{id:length(24)}")]
        public StandardReturn DeleteIdiom(string id, [FromBody] string openId)
        {
            //try
            //{
                return data.DeleteIdiom(id, openId);
            //}
            //catch (System.Exception)
            //{
            //    return new StandardReturn(-1);
            //    throw;
            //}
        }
        [HttpGet("search/{str:length(2,12)}")]
        public StandardReturn GetListByStr(string str)
        {
            try
            {
                return data.GetListByStr(str);
            }
            catch (System.Exception)
            {
                return new StandardReturn(-1);
                throw;
            }
        }
        [HttpGet("search/{id:length(24)}")]
        public StandardReturn GetListById(string id)
        {
            try
            {
                return data.GetListById(id);
            }
            catch (System.Exception)
            {
                return new StandardReturn(-1);
                throw;
            }
        }
        [HttpGet("search/{index:length(1)}")]
        public StandardReturn GetListByIndex(char index)
        {
            try
            {
                return data.GetListByIndex(char.ToUpper(index));
            }
            catch (System.Exception)
            {
                return new StandardReturn(-1);
                throw;
            }
        }
        [HttpGet("playsolitaire/{str:length(2,12)}")]
        public StandardReturn PlaySolitaire(string str)
        {
            try
            {
                return data.PlaySolitaire(str);
            }
            catch (System.Exception)
            {
                return new StandardReturn(-1);
                throw;
            }
        }
    }
}