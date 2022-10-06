using Otto.models;
using Otto.products.DTO;

namespace Otto.products.Models.Responses
{
    public class MItemsSearchResponse
    {
        public MItemsSearchResponse(ResponseCode r, string v, MItemsSearchDTO _mItemsSearch)
        {
            res = r;
            msg = v;
            mItemsSearch = _mItemsSearch;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public MItemsSearchDTO mItemsSearch { get; set; }
    }
}
