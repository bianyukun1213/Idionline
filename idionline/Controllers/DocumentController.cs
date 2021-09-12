using System;
using System.IO;
using System.Text;
using Idionline.Models;
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
                StreamReader sr = new(AppContext.BaseDirectory + "document.md", Encoding.UTF8);
                string text = sr.ReadToEnd();
                return new StandardReturn(result: text);
            }
            catch
            {
                return new StandardReturn(20001);
                throw;
            }
        }
    }
}
