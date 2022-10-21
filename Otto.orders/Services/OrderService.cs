using Microsoft.EntityFrameworkCore;
using Otto.models;
using Otto.orders.DTOs;
using Otto.orders.Mapper;
using System.ComponentModel.Design;

namespace Otto.orders.Services
{
    public class OrderService
    {
        public OrderService()
        {

        }

        public async Task<List<Order>> GetAsync()
        {
            using (var db = new OttoDbContext())
            {
                return await db.Orders.ToListAsync();
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            using (var db = new OttoDbContext())
            {
                return await db.Orders.FindAsync(id);
            }
        }

        public async Task<Order> GetByMOrderIdAsync(string id)
        {
            using (var db = new OttoDbContext())
            {
                var order = await db.Orders.Where(t => t.MOrderId == long.Parse(id)).FirstOrDefaultAsync();
                return order;
            }
        }

        public async Task<List<Order>> GetOrdersByPackId(string id)
        {
            using (var db = new OttoDbContext())
            {
                var orders = await db.Orders.Where(t => t.PackId == id).ToListAsync();
                return orders;
            }
        }

        public async Task<Order> GetByMOrderIdWithoutPackIdAsync(long id)
        {
            using (var db = new OttoDbContext())
            {
                var order = await db.Orders.Where(t => t.MOrderId == id).FirstOrDefaultAsync();
                return order;
            }
        }


        public async Task<List<PackDTO>> GetOrdersAsync(int companyId)
        {
            using(var db = new OttoDbContext())
            {
                var orders = await db.Orders.Where(t => t.CompanyId == companyId).ToListAsync();

                //List<OrderDTO> result = GetListOrderDTO(orders);

                List<PackDTO> otroResult = GetListPackDTO(orders);

                return otroResult;
            }
        }

        public async Task<List<PackDTO>> GetOrdersAsync(int companyId, OrderState state)
        {
            using (var db = new OttoDbContext())
            {
                var orders = await db.Orders.Where(t => t.State == state).ToListAsync();

                //List<OrderDTO> result = GetListOrderDTO(orders);

                List<PackDTO> otroResult = GetListPackDTO(orders);

                return otroResult;
            }
        }


        public async Task<List<PackDTO>> GetOrderByPackIdAsync(int companyId, string packId)
        {
            using (var db = new OttoDbContext())
            {
                var orders = await db.Orders.Where(t => t.CompanyId == companyId && t.PackId == packId).ToListAsync();

                //List<Order> result = GetListOrderDTO(orders);

                List<PackDTO> otroResult = GetListPackDTO(orders);

                return otroResult;
            }
        }


        private List<PackDTO> GetListPackDTO(List<Order> orders)
        {
            //agrupar por packid

            var result = new List<PackDTO>();

            var groupByPackIdQuery =
                                    from order in orders
                                    group order by order.PackId into newGroup
                                    orderby newGroup.Key
                                    select newGroup;

            foreach (var nameGroup in groupByPackIdQuery)
            {
                //var items = new List<OrderDTO>();
                var items = new List<Order>();
                //ordenes sin pack id
                //if (string.IsNullOrEmpty(nameGroup.Key)) 
                //{
                //    foreach (var order in nameGroup)
                //    {
                //        items = new List<OrderDTO>();
                //        items.Add(order);
                //        result.Add(new PackDTO(order.MOrderId.ToString(), "",  items));
                //    }
                //}
                //else 
                //{
                foreach (var order in nameGroup)
                {
                    items.Add(order);
                }
                result.Add(new PackDTO(nameGroup.Key, items));
                //}
            }
            return result;
        }

        private static List<OrderDTO> GetListOrderDTO(List<Order> orders)
        {
            var result = new List<OrderDTO>();
            orders.ForEach(order => result.Add(OrderMapper.GetOrderDTO(order)));
            return result;
        }

        public async Task<OrderDTO> GetOrderInProgressByMOrderIdAsync(string id, int userIdInProgress)
        {
            using (var db = new OttoDbContext())
            {
                var order = await db.Orders.Where(t => t.MOrderId == long.Parse(id) && 
                                                       t.State == OrderState.Tomada &&
                                                       t.InProgress == true &&
                                                       t.UserIdInProgress == userIdInProgress).FirstOrDefaultAsync();
                
                return order != null ? OrderMapper.GetOrderDTO(order) : null;                
            }
        }

        public async Task<PackDTO> GetOrderInProgressByPackIdAsync(string packId, int userIdInProgress)
        {
            using (var db = new OttoDbContext())
            {
                var orders = await db.Orders.Where(t => t.PackId == packId &&
                                                       t.State == OrderState.Tomada &&
                                                       t.InProgress == true &&
                                                       t.UserIdInProgress == userIdInProgress).ToListAsync();

                //var items = new List<OrderDTO>();
                var items = new List<Order>();
                foreach (var order in orders)
                {
                    //var dto = OrderMapper.GetOrderDTO(order);
                    items.Add(order);
                }
                return new PackDTO(packId, items);
            }
        }



        public async Task<Tuple<Order, int>> CreateAsync(Order order)
        {
            try
            {
                using (var db = new OttoDbContext())
                {
                    await db.Orders.AddAsync(order);
                    var rowsAffected = await db.SaveChangesAsync();


                    return new Tuple<Order, int>(order, rowsAffected);
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine($"Ya existe una orden con ese mOrdenId, se descarta el alta");
                return new Tuple<Order, int>(order, 1);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error al dar de alta la orden {ex}");
                throw;
            }            
        }
        public async Task<Tuple<OrderDTO, int>> UpdateOrderInProgressByMOrderIdAsync(string id, int UserIdInProgress)
        {
            //TODO check con el servicio de usuario con el id UserIdInProgress exista

            using (var db = new OttoDbContext())
            {
                var order = await db.Orders.Where(t => t.MOrderId == long.Parse(id) && t.InProgress == false).FirstOrDefaultAsync();
                if (order != null)
                {
                    UpdateOrderInProgressProperties(UserIdInProgress, order);
                    UpdateDateTimeKindForPostgress(order);
                }
                else
                {
                    return new Tuple<OrderDTO, int>(null, 0);
                }

                db.Entry(order).State = EntityState.Modified;
                var rowsAffected = await db.SaveChangesAsync();
                var dto = OrderMapper.GetOrderDTO(order);

                return new Tuple<OrderDTO, int>(dto, rowsAffected);
            }
        }

        public async Task<Tuple<PackDTO, int, string>> UpdateOrderInProgressByPackIdAsync(int companyId, string packId, int UserIdInProgress)
        { 
            using (var db = new OttoDbContext())
            {
                //check con el servicio de usuario con el id UserIdInProgress exista
                var user = await db.Users.Where(u => u.Id == UserIdInProgress).FirstOrDefaultAsync();
                if (user == null)
                    return new Tuple<PackDTO, int, string>(null, 0, "No existe el usuario que quiere tomar la orden");
                else if (user.CompanyId != companyId)
                    return new Tuple<PackDTO, int, string>(null, 0, "El usuario que quiere tomar la orden no pertenece a ese fullfilment");

                //var orders = await db.Orders.Where(t => t.PackId == packId && t.InProgress == false).ToListAsync();
                var orders = await db.Orders.Where(t => t.PackId == packId).ToListAsync();
                if (orders == null)
                    return new Tuple<PackDTO, int, string>(null, 0, "No se encontro una orden con ese packId");
                else if (orders!= null && orders.Any(o=> o.InProgress == true)) 
                    return new Tuple<PackDTO, int, string>(null, 0, "Un item de la orden ya se encuentra tomado");
                // check if that pack id, belongs to that company
                else if(orders != null && orders.Any(o => o.CompanyId != companyId))
                    return new Tuple<PackDTO, int, string>(null, 0, "Un item de la orden no pertenece a ese fullfilment");

                else
                {
                    int rowsAffected = 0;
                    //var items = new List<OrderDTO>();
                    var items = new List<Order>();
                    foreach (var order in orders)
                    {
                        UpdateOrderInProgressProperties(UserIdInProgress, order);
                        UpdateDateTimeKindForPostgress(order);
                        db.Entry(order).State = EntityState.Modified;
                        rowsAffected = +await db.SaveChangesAsync();
                        //var dto = OrderMapper.GetOrderDTO(order);
                        items.Add(order);
                    }

                    return new Tuple<PackDTO, int, string>(new PackDTO(packId, items), rowsAffected, "Ok");
                }                       
            }
        }

        public async Task<Tuple<PackDTO, int, string>> UpdateOrderStopInProgressByPackIdAsync(int companyId, string packId, int UserIdInProgress)
        {
            using (var db = new OttoDbContext())
            {
                //check con el servicio de usuario con el id UserIdInProgress exista
                var user = await db.Users.Where(u => u.Id == UserIdInProgress).FirstOrDefaultAsync();
                if (user == null)
                    return new Tuple<PackDTO, int, string>(null, 0, "No existe el usuario que quiere tomar la orden");
                else if (user.CompanyId != companyId)
                    return new Tuple<PackDTO, int, string>(null, 0, "El usuario que quiere dejar de tomar la orden no pertenece a ese fullfilment");

                int rowsAffected = 0;

                //quien esta cancelando sea quien la tomo
                var orders = await db.Orders.Where(t => t.PackId == packId &&
                                                       t.InProgress == true &&
                                                       t.UserIdInProgress == UserIdInProgress &&
                                                       t.State != OrderState.Finalizada &&
                                                       t.State != OrderState.Cancelada &&
                                                       t.State != OrderState.Enviada
                                                       ).ToListAsync();
                if (orders == null)
                    return new Tuple<PackDTO, int, string>(null, 0, "No se encontro la orden. Verificar que el usuario sea el correcto o que el estado de la orden sea el esperado");
                else 
                {
                    //var items = new List<OrderDTO>();
                    var items = new List<Order>();
                    foreach (var order in orders)
                    {
                        UpdateOrderInProgressProperties(UserIdInProgress, order, true);
                        UpdateDateTimeKindForPostgress(order);
                        db.Entry(order).State = EntityState.Modified;
                        rowsAffected = +await db.SaveChangesAsync();
                        //var dto = OrderMapper.GetOrderDTO(order);
                        items.Add(order);
                    }
                    return new Tuple<PackDTO, int, string>(new PackDTO(packId, items), rowsAffected,"Ok");
                }       
            }
        }


        public async Task<Tuple<OrderDTO, int>> UpdateFinalizeOrderByMOrderIdAsync(string id, int UserIdInProgress)
        {
            //TODO check con el servicio de usuario con el id UserIdInProgress exista

            using (var db = new OttoDbContext())
            {
                //quien esta cancelando sea quien la tomo
                var order = await db.Orders.Where(t => t.MOrderId == long.Parse(id) &&
                                                       t.InProgress == true &&
                                                       t.UserIdInProgress == UserIdInProgress &&
                                                       t.State != OrderState.Finalizada &&
                                                       t.State != OrderState.Cancelada &&
                                                       t.State != OrderState.Enviada
                                                       ).FirstOrDefaultAsync();
                if (order != null)
                {
                    UpdateFinalizeOrder(UserIdInProgress, order);
                    UpdateDateTimeKindForPostgress(order);
                }
                else
                {
                    return new Tuple<OrderDTO, int>(null, 0);
                }

                db.Entry(order).State = EntityState.Modified;
                var rowsAffected = await db.SaveChangesAsync();
                var dto = OrderMapper.GetOrderDTO(order);

                return new Tuple<OrderDTO, int>(dto, rowsAffected);
            }
        }

        public async Task<Tuple<PackDTO, int, string>> UpdateFinalizeOrderByPackIdAsync(int companyId, string packId, int UserIdInProgress)
        {
            //TODO check con el servicio de usuario con el id UserIdInProgress exista

            using (var db = new OttoDbContext())
            {
                int rowsAffected = 0;
                //quien esta cancelando sea quien la tomo
                var orders = await db.Orders.Where(t => t.PackId == packId &&
                                                       t.InProgress == true &&
                                                       t.UserIdInProgress == UserIdInProgress &&
                                                       t.State != OrderState.Finalizada &&
                                                       t.State != OrderState.Cancelada &&
                                                       t.State != OrderState.Enviada
                                                       ).ToListAsync();
                if (orders != null && orders.Count>0)
                {
                    //var items = new List<OrderDTO>();
                    var items = new List<Order>();
                    foreach (var order in orders)
                    {
                        UpdateFinalizeOrder(UserIdInProgress, order);
                        UpdateDateTimeKindForPostgress(order);
                        db.Entry(order).State = EntityState.Modified;
                        rowsAffected = +await db.SaveChangesAsync();
                        //var dto = OrderMapper.GetOrderDTO(order);
                        items.Add(order);
                    }
                    return new Tuple<PackDTO, int, string>(new PackDTO(packId, items), rowsAffected, "Ok");
                }
                else
                {
                    return new Tuple<PackDTO, int, string>(null, 0,"No se encontro una orden tomada por ese usuario con ese id");
                }
            }
        }

        private void UpdateFinalizeOrder(int UserIdInProgress, Order order)
        {
            var utcNow = DateTime.UtcNow;
            order.Modified = utcNow;
            order.State = OrderState.Finalizada;
            order.InProgress = false;
            order.InProgressDateTimeModified = utcNow;
        }


        private void UpdateOrderInProgressProperties(int UserIdInProgress, Order order,bool CancelInProgress = false)
        {
            var utcNow = DateTime.UtcNow;
            if (CancelInProgress)
            {
                order.Modified = utcNow;
                order.State = OrderState.Pendiente;
                order.InProgress = false;
                order.InProgressDateTimeTaken = utcNow;
                order.InProgressDateTimeModified = utcNow;
            }
            else 
            {
                order.Modified = utcNow;
                order.State = OrderState.Tomada;
                order.InProgress = true;
                order.UserIdInProgress = UserIdInProgress;
                order.InProgressDateTimeTaken = utcNow;
                order.InProgressDateTimeModified = utcNow;
            }

        }

        public async Task<Tuple<Order, int>> UpdateAsync(long id, Order newOrder)
        {
            try
            {
                using (var db = new OttoDbContext())
                {
                    // Si ya existe un token con ese mismo usuario, hago el update
                    var order = await db.Orders.Where(t => t.Id == id).FirstOrDefaultAsync();
                    if (order != null)
                    {
                        UpdateOrderProperties(newOrder, order);
                        UpdateDateTimeKindForPostgress(order);
                    }

                    db.Entry(order).State = EntityState.Modified;
                    var rowsAffected = await db.SaveChangesAsync();
                    return new Tuple<Order, int>(order, rowsAffected);
                }
            }
            catch (Exception ex )
            {
                var a = ex;
                throw;
            }           
        }

        public async Task<Tuple<Order, int>> UpdateOrderTableByIdAsync(int id, Order newOrder)
        {
            using (var db = new OttoDbContext())
            {
                // Si ya existe un token con ese mismo usuario, hago el update
                var order = await db.Orders.Where(t => t.Id == id).FirstOrDefaultAsync();
                if (order != null)
                {

                    var utcNow = DateTime.UtcNow;
                    order.Modified = DateTime.UtcNow;

                    UpdateOrderAllProperties(newOrder, order);
                    UpdateDateTimeKindForPostgress(order);
                }

                db.Entry(order).State = EntityState.Modified;
                var rowsAffected = await db.SaveChangesAsync();
                return new Tuple<Order, int>(order, rowsAffected);
            }
        }

        public async Task<Tuple<Order, int>> UpdateOrderTableBySalesChannelOrderIdAsync(long SalesChannelOrderId, Order newOrder, bool isTiendanube = false, string productId = "")
        {
            try
            {
                using (var db = new OttoDbContext())
                {
                    Order order = isTiendanube ? await db.Orders.Where(t => t.TOrderId == SalesChannelOrderId && t.ItemId == productId).FirstOrDefaultAsync()
                                               : await db.Orders.Where(t => t.MOrderId == SalesChannelOrderId).FirstOrDefaultAsync();

                    if (order != null)
                    {

                        var utcNow = DateTime.UtcNow;
                        order.Modified = DateTime.UtcNow;

                        UpdateOrderChangedProperties(newOrder, order);
                        UpdateDateTimeKindForPostgress(order);
                    }

                    db.Entry(order).State = EntityState.Modified;
                    var rowsAffected = await db.SaveChangesAsync();
                    return new Tuple<Order, int>(order, rowsAffected);
                }
            }
            catch (Exception ex )
            {
                var j = ex;
                throw;
            }            
        }

        private void UpdateOrderAllProperties(Order newOrder, Order order)
        {
            var utcNow = DateTime.UtcNow;
            order.UserId = newOrder.UserId;

            order.UserName = newOrder.UserName;
            order.UserLastName = newOrder.UserLastName;

            order.MUserId = newOrder.MUserId;
            order.MOrderId = newOrder.MOrderId;
            order.CompanyId = newOrder.CompanyId;
            order.ItemId = newOrder.ItemId;
            order.ItemDescription = newOrder.ItemDescription;
            order.Quantity = newOrder.Quantity;
            order.PackId = newOrder.PackId;
            order.SKU = newOrder.SKU;
            order.ShippingStatus = newOrder.ShippingStatus;
            order.Created = newOrder.Created;
            order.Modified = utcNow;
            order.State = newOrder.State;
            if(newOrder.InProgress != null)                 
                order.InProgress = newOrder.InProgress;
            order.UserIdInProgress = newOrder.UserIdInProgress;
            order.InProgressDateTimeTaken = newOrder.InProgressDateTimeTaken;
            order.InProgressDateTimeModified = newOrder.InProgressDateTimeModified;
        }
        private void UpdateOrderChangedProperties(Order newOrder, Order order)
        {
            var utcNow = DateTime.UtcNow;
            if (newOrder.UserId != null && order.UserId != null && order.UserId != newOrder.UserId)
                order.UserId = newOrder.UserId;

            if (order.UserName != null && !string.IsNullOrEmpty(newOrder.UserName) && order.UserName != newOrder.UserName)
                order.UserName = newOrder.UserName;

            if (order.UserLastName != null && !string.IsNullOrEmpty(newOrder.UserLastName) && order.UserLastName != newOrder.UserLastName)
                order.UserLastName = newOrder.UserLastName;

            if (order.PackId != null && !string.IsNullOrEmpty(newOrder.PackId) && order.PackId != newOrder.PackId)
                order.PackId = newOrder.PackId;

            if (order.MUserId != null && newOrder.MUserId != null && order.MUserId != newOrder.MUserId)
                order.MUserId = newOrder.MUserId;

            if (order.MOrderId != null && newOrder.MOrderId != null &&  order.MOrderId != newOrder.MOrderId)
                order.MOrderId = newOrder.MOrderId;

            if (order.TUserId != null && newOrder.TUserId != null && order.TUserId != newOrder.TUserId)
                order.TUserId = newOrder.TUserId;

            if (order.TOrderId != null && newOrder.TOrderId != null && order.TOrderId != newOrder.TOrderId)
                order.TOrderId = newOrder.TOrderId;

            if (order.CompanyId != null && newOrder.CompanyId != null && order.CompanyId != newOrder.CompanyId)
                order.CompanyId = newOrder.CompanyId;

            if (order.ItemId != null && newOrder.ItemId != null &&  order.ItemId != newOrder.ItemId)
                order.ItemId = newOrder.ItemId;

            if (order.ItemDescription != null && newOrder.ItemDescription != null && order.ItemDescription != newOrder.ItemDescription)
                order.ItemDescription = newOrder.ItemDescription;

            if (order.Quantity != newOrder.Quantity)
                order.Quantity = newOrder.Quantity;

            if (order.PackId != null && !string.IsNullOrEmpty(newOrder.PackId) && order.PackId != newOrder.PackId)
                order.PackId = newOrder.PackId;

            if (order.SKU != null && newOrder.SKU != null && order.SKU != newOrder.SKU)
                order.SKU = newOrder.SKU;

            if (order.ShippingStatus != newOrder.ShippingStatus)
                order.ShippingStatus = newOrder.ShippingStatus;
            //if (order.Created != null && order.Created != newOrder.Created)
            //    order.Created = newOrder.Created;

            if (order.Modified != null && order.Modified != utcNow)
                order.Modified = utcNow;

            if (order.State != newOrder.State)
                order.State = newOrder.State;

            if (order.UserIdInProgress != null && newOrder.UserIdInProgress != null && order.UserIdInProgress != newOrder.UserIdInProgress)
                order.UserIdInProgress = newOrder.UserIdInProgress;

            if (order.InProgressDateTimeTaken != null && newOrder.InProgressDateTimeTaken != null &&  order.InProgressDateTimeTaken != newOrder.InProgressDateTimeTaken)
                order.InProgressDateTimeTaken = newOrder.InProgressDateTimeTaken;

            if (order.InProgressDateTimeModified != null && newOrder.InProgressDateTimeModified != null &&  order.InProgressDateTimeModified != newOrder.InProgressDateTimeModified)
                order.InProgressDateTimeModified = newOrder.InProgressDateTimeModified;

            if (newOrder.InProgress != null && order.InProgress != null && order.InProgress != newOrder.InProgress)
                order.InProgress = newOrder.InProgress;

            if (newOrder.StateDescription != null && order.StateDescription != null && order.StateDescription != newOrder.StateDescription)
                order.StateDescription = newOrder.StateDescription;
        }



        private void UpdateOrderProperties(Order newOrder, Order order)
        {
            var utcNow = DateTime.UtcNow;
            order.Modified = DateTime.UtcNow;
        }

        private void UpdateDateTimeKindForPostgress(Order order)
        {
            order.Modified = DateTime.SpecifyKind((DateTime)order.Modified, DateTimeKind.Utc);
        }

        public async Task<int> DeleteAsync(int id, Order delOrder)
        {
            using (var db = new OttoDbContext())
            {
                var rowsAffected = 0;
                var order = await db.Orders.FindAsync(id);
                if (order != null)
                {
                    db.Orders.Remove(delOrder);
                    rowsAffected = await db.SaveChangesAsync();
                }

                return rowsAffected;
            }

        }

        public async Task<Order> GetOrderByTOrderIdAndItemId(int orderId, string productId)
        {
            using (var db = new OttoDbContext())
            {
                var order = await db.Orders.Where(t => t.TOrderId == orderId && t.ItemId == productId).FirstOrDefaultAsync();
                return order;
            }
        }
    }
}
