using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Idionline.Models;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdiomController : ControllerBase
    {
        private readonly IdionlineContext _context;
        public IdiomController(IdionlineContext context)
        {
            _context = context;
            if (_context.Idioms.Count() == 0)
            {
                _context.Idioms.Add(new Idiom { IdiomName = "成语名称", Id = 1, Interpretation = "成语释义JSON", Source = "《现代汉语词典》第七版", LastEditor = "最后编辑者", UpdateTimeUT = DateTimeOffset.MinValue.ToUnixTimeSeconds(), Index = 'A' });
                _context.SaveChanges();
            }
        }
        //[HttpGet]
        //public ActionResult<List<Idiom>> GetAll()
        //{
        //    return _context.Idioms.ToList();
        //}
        [HttpGet("count")]
        public ActionResult<int> GetCount()
        {
            int count = _context.Idioms.Count<Idiom>();
            return count;
        }
        [HttpGet("{id}")]
        public ActionResult<Idiom> GetById(int id)
        {
            var item = _context.Idioms.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpGet("search/{name}")]
        public ActionResult<Dictionary<string, int>> SearchByName(string name)
        {
            var items = from m in _context.Idioms select m;
            if (!string.IsNullOrEmpty(name))
            {
                items = items.Where(s => s.IdiomName.Contains(name));
                if (items.Count() != 0)
                {
                    Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
                    foreach (var item in items)
                    {
                        keyValuePairs.Add(item.IdiomName, item.Id);
                    }
                    return keyValuePairs;
                }
            }
            return NotFound();
        }
        [HttpGet("index/{index}")]
        public ActionResult<Dictionary<string, int>> SearchByIndex(char index)
        {
            var items = from m in _context.Idioms select m;
            if (char.IsLetter(index))
            {
                items = items.Where(s => s.Index.Equals(char.ToUpper(index)));
                if (items.Count() != 0)
                {
                    Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
                    foreach (var item in items)
                    {
                        keyValuePairs.Add(item.IdiomName, item.Id);
                    }
                    return keyValuePairs;
                }
            }
            return NotFound();
        }
    }
}
