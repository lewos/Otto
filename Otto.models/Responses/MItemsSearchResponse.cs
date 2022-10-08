using Otto.models;

namespace Otto.models.Responses
{
    public class MItemsSearchResponse<T>
    {
        public MItemsSearchResponse(ResponseCode r, string v, T _mItemsSearch)
        {
            res = r;
            msg = v;
            mItemsSearch = _mItemsSearch;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T mItemsSearch { get; set; }
    }
}
