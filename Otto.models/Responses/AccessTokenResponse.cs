using Otto.models;

namespace Otto.models.Responses
{
    public class AccessTokenResponse<T>
    {
        public AccessTokenResponse(ResponseCode r, string v, T mtoken)
        {
            res = r;
            msg = v;
            token = mtoken;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T token { get; set; }

    }
}
