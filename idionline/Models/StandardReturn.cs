namespace Idionline.Models
{
    public class StandardReturn
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object Result { get; set; }
        public StandardReturn(int code = 0, object result = null)
        {
            switch (code)
            {
                //1：系统，2：数据库，3：第三方。
                case 0:
                    Code = 0;
                    Msg = "成功";
                    Result = result;
                    break;
                case -1:
                    Code = -1;
                    Msg = "未知错误";
                    break;
                case 20001:
                    Code = 20001;
                    Msg = "未查找到数据";
                    break;
                case 20002:
                    Code = 20002;
                    Msg = "数据格式不正确，无法更新数据";
                    break;
                case 20003:
                    Code = 20003;
                    Msg = "无操作权限，无法更新数据";
                    break;
                case 20004:
                    Code = 20004;
                    Msg = "不允许重复提交，无法更新数据";
                    break;
                case 30001:
                    Code = 30001;
                    Msg = "第三方服务错误";
                    break;
            }
        }
    }
}