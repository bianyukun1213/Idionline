using Idionline.Resources;
using Microsoft.Extensions.Localization;

namespace Idionline.Models
{
    public class StandardReturn
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object Result { get; set; }
        public StandardReturn(IStringLocalizer<SharedResource> localizer, int code = 0, object result = null)
        {
            switch (code)
            {
                //1：系统，2：数据库，3：第三方。
                case 0:
                    Code = 0;
                    Msg = localizer["succeeded"];
                    Result = result;
                    break;
                case -1:
                    Code = -1;
                    Msg = localizer["unknownError"];
                    break;
                case 20001:
                    Code = 20001;
                    Msg = localizer["dataNotFound"];
                    break;
                case 20002:
                    Code = 20002;
                    Msg = localizer["wrongData"];
                    break;
                case 20003:
                    Code = 20003;
                    Msg = localizer["permissionDenied"];
                    break;
                case 20004:
                    Code = 20004;
                    Msg = localizer["repeatedSubmissionNotAllowed"];
                    break;
                case 30001:
                    Code = 30001;
                    Msg = localizer["thirdPartyServiceError"];
                    break;
            }            
        }
    }
}