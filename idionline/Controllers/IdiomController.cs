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
                _context.Idioms.Add(new Idiom { IdiomName = "成语名称", ID = 1, Interpretation = "成语释义JSON", Source = "《现代汉语词典》第七版", UpdateDate = DateTime.Now.Date, Index = 'A' });
                _context.SaveChanges();
            }
        }
        [HttpGet]
        public ActionResult<List<Idiom>> GetAll()
        {
            return _context.Idioms.ToList();
        }
        [HttpGet("{id}", Name = "GetIdiom")]
        public ActionResult<Idiom> GetById(int id)
        {
            var item = _context.Idioms.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpGet("search/{name}", Name = "SearchIdiom")]
        public ActionResult<List<Idiom>> SearchByName(string name)
        {
            var items = from m in _context.Idioms select m;
            if (!String.IsNullOrEmpty(name))
            {
                items = items.Where(s => s.IdiomName.Contains(name));
                return items.ToList();
            }
            return NotFound();
        }
        [HttpGet("index/{index}", Name = "SearchIdiomByIndex")]
        public ActionResult<List<Idiom>> SearchByIndex(char index)
        {
            var items = from m in _context.Idioms select m;
            if (Char.IsLetter(index))
            {
                items = items.Where(s => s.Index.Equals(Char.ToUpper(index)));
                return items.ToList();
            }
            return NotFound();
        }
    }
}
