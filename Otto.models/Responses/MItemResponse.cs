using Otto.models;

namespace Otto.models.Responses
{
    public class MItemResponse<T>
    {
        public MItemResponse(ResponseCode r, string v, T mItem)
        {
            res = r;
            msg = v;
            mItem = mItem;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T mItem { get; set; }
    }
}
