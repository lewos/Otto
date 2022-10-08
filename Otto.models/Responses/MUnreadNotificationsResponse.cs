using Otto.models;

namespace Otto.models.Responses
{
    public class MUnreadNotificationsResponse<T>
    {
        public MUnreadNotificationsResponse(ResponseCode r, string v, T missedFeedsDTO)
        {
            res = r;
            msg = v;
            missedFeeds = missedFeedsDTO;
        }

        public ResponseCode res { get; set; }

        public string msg { get; set; }

        public T missedFeeds { get; set; }

    }
}
