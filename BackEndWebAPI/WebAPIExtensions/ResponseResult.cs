namespace BackEndWebAPI.WebAPIExtensions
{
    //可序列化特性
    [Serializable]
    public class ResponseResult<T>
    {
        private int code;
        private string msg;
        private T data;

        public ResponseResult()
        {
            //默认的code 和msg
            this.code = (int)AppHttpCodeEnum.SUCCESS;
            this.msg = AppHttpCodeEnum.SUCCESS.ToString();
        }

        public ResponseResult(int code, T data)
        {
            this.code = code;
            this.data = data;
        }

        public ResponseResult(int code, string msg, T data)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }

        public ResponseResult(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public static ResponseResult<T> ErrorResult(int code, string msg)
        {
            var result = new ResponseResult<T>();
            return result.Error(code, msg);
        }
        public static ResponseResult<T> OkResult()
        {
            var result = new ResponseResult<T>();
            return result;
        }

        public static ResponseResult<T> OkResult(int code, string msg)
        {
            var result = new ResponseResult<T>();
            return result.Ok(code, default(T), msg);
        }

        public static ResponseResult<T> OkResult(T data)
        {
            var result = SetAppHttpCodeEnum(AppHttpCodeEnum.SUCCESS, AppHttpCodeEnum.SUCCESS.ToString());
            if (data != null)
            {
                result.data = data;
            }
            return result;
        }

        public static ResponseResult<T> ErrorResult(AppHttpCodeEnum enums)
        {
            return SetAppHttpCodeEnum(enums, enums.ToString());
        }

        public static ResponseResult<T> ErrorResult(AppHttpCodeEnum enums, string msg)
        {
            return SetAppHttpCodeEnum(enums, msg);
        }

        public static ResponseResult<T> SetAppHttpCodeEnum(AppHttpCodeEnum enums)
        {
            return OkResult((int)enums, enums.ToString());
        }

        private static ResponseResult<T> SetAppHttpCodeEnum(AppHttpCodeEnum enums, string msg)
        {
            return OkResult((int)enums, msg);
        }

        public ResponseResult<T> Error(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
            return this;
        }

        public ResponseResult<T> Ok(int code, T data)
        {
            this.code = code;
            this.data = data;
            return this;
        }

        public ResponseResult<T> Ok(int code, T data, string msg)
        {
            this.code = code;
            this.data = data;
            this.msg = msg;
            return this;
        }

        public ResponseResult<T> Ok(T data)
        {
            this.data = data;
            return this;
        }
        // 直接定义为属性比较好,下面写法不符合C#的习惯
        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        public T Data
        {
            get { return data; }
            set { data = value; }
        }
    }

}
