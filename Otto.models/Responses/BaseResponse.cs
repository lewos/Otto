using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.models.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse(ResponseCode r, string v, T c)
        {
            res = r;
            msg = v;
            content = c;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T content { get; set; }

    }
}
