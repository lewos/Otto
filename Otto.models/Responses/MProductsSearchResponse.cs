using Otto.models;

namespace Otto.models.Responses
{
    public class MProductsSearchResponse<T>
    {
        public MProductsSearchResponse(ResponseCode r, string v, List<T> _mProductsSearch)
        {
            res = r;
            msg = v;
            mProductsSearch = _mProductsSearch;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public List<T> mProductsSearch { get; set; }
    }
}
