using Otto.models;

namespace Otto.models.Responses
{
    public class MOrderResponse<T>
    {
        public MOrderResponse()
        {

        }
        public MOrderResponse(ResponseCode r, string v, T o)
        {
            res = r;
            msg = v;
            order = o;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T order { get; set; }
    }
}
