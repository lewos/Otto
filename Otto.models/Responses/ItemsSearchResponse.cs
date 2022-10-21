using Otto.models;

namespace Otto.models.Responses
{
    public class ItemsSearchResponse<T>
    {
        public ItemsSearchResponse(ResponseCode r, string v, T _itemsSearch)
        {
            res = r;
            msg = v;
            itemsSearch = _itemsSearch;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T itemsSearch { get; set; }
    }
}
