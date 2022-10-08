using Otto.models;
using Otto.orders.DTOs;

namespace Otto.orders.Mapper
{
    public static class OrderMapper
    {
        public static Order GetOrder(OrderDTO dto)
        {
            var capitalizedState = "";
            if (!string.IsNullOrEmpty(dto.State))
            {
                capitalizedState = dto.State.ToUpper()[0] + dto.State.ToLower().Substring(1);
            }

            Enum.TryParse(capitalizedState, out OrderState state);

            var capitalizedShippingStatus = "";
            if (!string.IsNullOrEmpty(dto.State))
            {
                capitalizedShippingStatus = dto.State.ToUpper()[0] + dto.State.ToLower().Substring(1);
            }

            Enum.TryParse(capitalizedShippingStatus, out State shippingStatus);


            return new Order
            {
                Id = dto.Id,
                UserId = dto.UserId,
                MUserId = dto.MUserId,
                MOrderId = dto.MOrderId,
                CompanyId = dto.CompanyId,
                ItemId = dto.ItemId,
                MShippingId = dto.MShippingId,
                ItemDescription = dto.ItemDescription,
                Quantity = dto.Quantity,
                PackId = dto.PackId,
                SKU = dto.SKU,
                ShippingStatus = shippingStatus,
                Created = dto.Created,
                Modified = dto.Modified,
                State = state,
                InProgress = dto.InProgress,
                UserIdInProgress = dto.UserIdInProgress,
                InProgressDateTimeTaken = dto.InProgressDateTimeTaken,
                InProgressDateTimeModified = dto.InProgressDateTimeModified
            };
        }


        public static OrderDTO GetOrderDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                MUserId = order.MUserId,
                MOrderId = order.MOrderId,
                MShippingId = order.MShippingId,
                CompanyId = order.CompanyId,
                ItemId = order.ItemId,
                ItemDescription = order.ItemDescription,
                Quantity = order.Quantity,
                PackId = order.PackId,
                SKU = order.SKU,
                ShippingStatus = order.ShippingStatus.ToString(),
                Created = order.Created,
                Modified = order.Modified,
                State = order.State.ToString(),
                InProgress = order.InProgress,
                UserIdInProgress = order.UserIdInProgress,
                InProgressDateTimeTaken = order.InProgressDateTimeTaken,
                InProgressDateTimeModified = order.InProgressDateTimeModified,
            };
        }
    }
}
