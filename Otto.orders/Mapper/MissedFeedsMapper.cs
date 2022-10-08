using Otto.orders.DTOs;

namespace Otto.orders.Mapper
{
    public static class MissedFeedsMapper
    {
        public static MOrderNotificationDTO GetMOrderNotificationDTO(Message message)
        {            
            return new MOrderNotificationDTO
            {
                MUserId = long.Parse(message.UserId),
                Id = message.Id,
                Resource = message.Resource,
                Topic = message.Topic,
                ApplicationId = message.ApplicationId,
                Attempts = message.Attempts,
                Sent = message.Sent,
                Received = message.Received,
            };
        }
        public static List<MOrderNotificationDTO> GetListMOrderNotificationDTO(List<Message> messages) 
        {
            var response = new List<MOrderNotificationDTO>();
            foreach (var message in messages)
                response.Add(GetMOrderNotificationDTO(message));
            return response;
        }
    }
}
