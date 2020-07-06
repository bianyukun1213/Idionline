using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idionline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        public StandardReturn GetHelp()
        {
            try
            {
                StreamReader sr = new StreamReader(AppContext.BaseDirectory + "document.md", Encoding.UTF8);
                string text = sr.ReadToEnd();
                return new StandardReturn(result: text);
            }
            catch
            {
                return new StandardReturn(20001);
            }
        }
    }
}
