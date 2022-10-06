using Otto.models;

namespace Otto.Models
{
    public class Response<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public T Content { get; set; }

        public Response(T content,string msg = null)
        {
            this.Code = content is not null ? ResponseCode.OK.ToString() : ResponseCode.ERROR.ToString();
            this.Message = this.Code.Equals(ResponseCode.OK.ToString()) ? ResponseCode.OK.ToString() : !string.IsNullOrEmpty(msg) ? msg: ResponseCode.ERROR.ToString();
            this.Content = content;
        }
    }
}
