using Otto.models;
using Otto.products.DTO;

namespace Otto.products.Models.Responses
{
    public class MProductsSearchResponse
    {
        public MProductsSearchResponse(ResponseCode r, string v, List<MProductsSearchDTO> _mProductsSearch)
        {
            res = r;
            msg = v;
            mProductsSearch = _mProductsSearch;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public List<MProductsSearchDTO> mProductsSearch { get; set; }
    }
}
