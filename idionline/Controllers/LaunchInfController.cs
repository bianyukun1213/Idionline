using System;
using System.Collections.Generic;
using System.Linq;
using Idionline.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaunchInfController : ControllerBase
    {
        private readonly IdionlineContext _context;
        public LaunchInfController(IdionlineContext context)
        {
            _context = context;
            if (_context.LaunchInfs.Count() == 0)
            {
                _context.LaunchInfs.Add(new LaunchInf { Text = "默认文本", DailyIdiom = "默认每日成语", DateUT = DateTimeOffset.MinValue.ToUnixTimeSeconds() });
                _context.SaveChanges();
            }
        }
        [HttpGet]
        public ActionResult<List<LaunchInf>> GetAll()
        {
            return _context.LaunchInfs.ToList();
        }
        [HttpGet("{date}", Name = "GetLaunchInf")]
        public ActionResult<List<LaunchInf>> GetLaunchInf(long date)
        {
            var common = _context.LaunchInfs.Find(DateTimeOffset.MinValue.ToUnixTimeSeconds());
            var item = _context.LaunchInfs.Find(date);
            if (common == null)
            {
                return NotFound();
            }
            List<LaunchInf> list = new List<LaunchInf>();
            list.Add(common);
            if (item!=null)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
