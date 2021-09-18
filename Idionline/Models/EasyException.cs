using System;

namespace Idionline.Models
{
    public class EasyException:Exception
    {
        public int ErrorCode { get; set; }
        public EasyException(int code)
        {
            ErrorCode = code;
        }
    }
}
