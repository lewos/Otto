using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.models.Responses
{
    public class MRefreshTokenResponse<T>
    {
        public MRefreshTokenResponse(ResponseCode r, string v, T mToken)
        {
            res = r;
            msg = v;
            token = mToken;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T token { get; set; }
    }
}
