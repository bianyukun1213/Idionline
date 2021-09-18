using System;
using System.IO;
using System.Text;
using Idionline.Models;
using Idionline.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Idionline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public DocumentController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }
        public StandardReturn GetHelp()
        {
            try
            {
                StreamReader sr = new(AppContext.BaseDirectory + "document.md", Encoding.UTF8);
                string text = sr.ReadToEnd();
                return new StandardReturn(result: text, localizer: _localizer);
            }
            catch
            {
                return new StandardReturn(code: 20001, localizer: _localizer);
                //throw;
            }
        }
    }
}
