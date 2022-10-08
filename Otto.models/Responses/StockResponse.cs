using Otto.models;

namespace Otto.models.Responses
{
    public class StockResponse<T>
    {

        public StockResponse(ResponseCode r, string v, List<T> _stockDTO)
        {
            res = r;
            msg = v;
            stockDTOs = _stockDTO;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public List<T> stockDTOs { get; set; }
    }
}