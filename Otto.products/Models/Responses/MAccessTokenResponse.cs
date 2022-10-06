using Otto.models;
using Otto.products.DTO;

namespace Otto.products.Models.Responses
{
    public class MAccessTokenResponse
    {
        public MAccessTokenResponse(ResponseCode r, string v, MTokenDTO mToken)
        {
            res = r;
            msg = v;
            token = mToken;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public MTokenDTO token { get; set; }
    }
}
