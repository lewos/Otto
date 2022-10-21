using Otto.models;
using Otto.models.Responses;
using Otto.orders.DTOs;
using Otto.orders.Mapper;

namespace Otto.orders.Services
{
    public class TOrdersService
    {
        private readonly AccessTokenService _accessTokenService;
        private readonly TiendanubeService _tiendanubeService;
        private readonly OrderService _orderService;
        private readonly UserService _userService;
        private readonly StockService _stockService;


        public TOrdersService(AccessTokenService accessTokenService, TiendanubeService tiendanubeService, OrderService orderService, UserService userService, StockService stockService)
        {
            _accessTokenService = accessTokenService;
            _tiendanubeService = tiendanubeService;
            _orderService = orderService;
            _userService = userService;
            _stockService = stockService;
        }

        public async Task<Tuple<Token, string>> CreateNewTTokenRegisterAsync(string code)
        {
            TCodeForTokenDTO tCodeForTokenDTO = await _tiendanubeService.GetTokenWithCodeAsync(code);

            if(!string.IsNullOrEmpty(tCodeForTokenDTO.Error))
                return new Tuple<Token, string>(null, tCodeForTokenDTO.ErrorDescription);

            var token = AccessTokenMapper.GetToken(tCodeForTokenDTO);

            var mAccessTokenResponse = await _accessTokenService.CreateNewRegisterAsync(token, "Tiendanube");
            if (mAccessTokenResponse.res == ResponseCode.OK)
                return new Tuple<Token, string>(mAccessTokenResponse.token, "Ok");
            else
                return new Tuple<Token, string>(null, "Error");
        }

        public async Task<bool> CreateOrderWebHook(Token token)
        {
            var res = await _tiendanubeService.CreateOrderWebHook((long)token.TUserId,token.AccessToken);
            return res.res == ResponseCode.OK;
        }

        public async Task<int> ProcesarOrden(TOrderNotificationDTO dto)
        {
            //Ver si el evento es el que necesito
            if (!string.IsNullOrEmpty(dto.Event) && dto.Event.Contains("order/paid"))
            {
                if (await isNewOrder(dto))
                    return (await CreateOrder(dto)).content;

                return (await UpdateOrder(dto)).content;
            }
            return 0;
        }

        private async Task<bool> isNewOrder(TOrderNotificationDTO dto)
        {
            try
            {
                var orders = await _orderService.GetOrdersByPackId(dto.Id.ToString());
                
                return orders.Count == 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<BaseResponse<int>> UpdateOrder(TOrderNotificationDTO dto)
        {
            var orderResponse = await GetTOrder(dto);
            if (orderResponse.res != ResponseCode.OK)
                return new BaseResponse<int>(orderResponse.res, orderResponse.msg, 0);

            return await UpdateOrderFields(orderResponse.content);
        }

        private async Task<BaseResponse<int>> UpdateOrderFields(TOrderDTO order)
        {
            // Buscar ese usuario id
            var user = await GetUser(order.StoreId.ToString());

            //Por cada producto buscar en el stock
            foreach (var product in order.Products)
            {               
                Order newOrder = GetNeWOrder(user, product, order);
                
                // ver como llega la orden cuando se cancela.



                //comparar datos y actualizar
                var algo = await _orderService.UpdateOrderTableBySalesChannelOrderIdAsync(order.Id, newOrder, true, product.ProductId.ToString());
                Console.WriteLine($"Cantidad de filas afectadas {algo.Item2}");
            }

            return new BaseResponse<int>(ResponseCode.OK, "", 0);
        }

        private async Task<BaseResponse<int>> CreateOrder(TOrderNotificationDTO dto)
        {
            var orderResponse = await GetTOrder(dto);
            if (orderResponse.res != ResponseCode.OK)
                return new BaseResponse<int>(orderResponse.res, orderResponse.msg, 0);

            return await CreateOrder(orderResponse.content);
        }

        private async Task<BaseResponse<TOrderDTO>> GetTOrder(TOrderNotificationDTO dto)
        {
            //Buscar el accessToken de ese usuario
            Token accessToken = await GetAccessToken(dto.StoreId,true);

            var orderResponse = new BaseResponse<TOrderDTO>(ResponseCode.ERROR, "", new TOrderDTO());

            if (accessToken != null)            
                orderResponse = await _tiendanubeService.GetTOrderAsync((long)dto.StoreId, dto.Event, dto.Id, accessToken.AccessToken);            

            return orderResponse;
        }

        private async Task<Token> GetAccessToken(long tUserId, bool isTiendanube = false)
        {
            var res = await _accessTokenService.GetTokenCacheAsync(tUserId, isTiendanube);
            return res.token;
        }

        private async Task<BaseResponse<int>> CreateOrder(TOrderDTO order)
        {
            // Buscar ese usuario id
            var user = await GetUser(order.StoreId.ToString());

            List<int> productsNotInStock = new List<int>();
            List<Tuple<Order, int>> registrosAfectados = new List<Tuple<Order, int>>();

            //Por cada producto buscar en el stock
            foreach (var product in order.Products)
            {
                int productInStockId = await GetProductInStockByItemId(product.ProductId.ToString(), user.Id, isTiendanube: true);
                if (productInStockId != 0)
                {
                    Order newOrder = GetNeWOrder(user, product, order, productInStockId);

                    Tuple<Order, int> respServ = await _orderService.CreateAsync(newOrder);

                    registrosAfectados.Add(respServ);
                }
                    
                productsNotInStock.Add(productInStockId);

            }
            LogProductsNotInStock(productsNotInStock, user.Id);

            var sum = 0;
            foreach (var reg in registrosAfectados) 
            {
                Console.WriteLine($"Cantidad de filas afectadas {reg.Item2}");
                sum += reg.Item2;
            }
            if(registrosAfectados.Count == sum)
                return new BaseResponse<int>(ResponseCode.OK, "", 1);

            return new BaseResponse<int>(ResponseCode.ERROR, "Verificar log", 0);



            //Ver que no exista una orden con esos datos para no duplicar
            //if (await isNewOrder(order.Id))

        }





        private async Task<User> GetUser(string SellerId)
        {
            var userResponse = await _userService.GetUserByMIdCacheAsync(SellerId, isTiendanube:true);
            return userResponse.res == ResponseCode.OK
                ? userResponse.user
                : null;

        }

        private async Task<int> GetProductInStockByItemId(string itemId, int userId, bool isTiendanube = false)
        {
            var tupleStockResponse = await _stockService.GetProductInStockByItemId(itemId, userId, isTiendanube: true);
            if (tupleStockResponse.Item1)
                return tupleStockResponse.Item2;
            return 0;
        }

        private Order GetNeWOrder(User user, Product product, TOrderDTO? order, int productInStockId = 0)
        {
            var utcNow = DateTime.UtcNow;
            var newOrder = new Order
            {
                UserId = user?.Id == null ? 0 : user.Id,
                UserName = user?.Name == null ? null : user.Name,
                UserLastName = user?.LastName == null ? null : user.LastName,

                TUserId = long.Parse(order.StoreId) == null ? null : long.Parse(order.StoreId),
                TOrderId = product?.Id == null ? null : product.Id,
                //TODO ver
                //TShippingId = order.Shipping?.Id == null ? null : order.Shipping.Id,

                CompanyId = user?.CompanyId == null ? null : user.CompanyId,
                ProductInStockId = productInStockId == 0 ? null : productInStockId,

                ItemId = product?.ProductId.ToString() == null ? null : product?.ProductId.ToString(),
                ItemDescription = product.Name == null ? null : product.Name,
                Quantity = int.Parse(product.Quantity),
                PackId = order.Id == null ? "Otto-T-" + Guid.NewGuid().ToString("n") : order.Id.ToString(),
                SKU = product.Sku == null ? null : product.Sku,
                State = OrderState.Pendiente,
                ShippingStatus = State.Pendiente,
                Created = utcNow,
                Modified = utcNow,
                InProgress = false,
            };

            return newOrder;
        }

        private void LogProductsNotInStock(List<int> productsNotInStock, long userId)
        {
            foreach (var product in productsNotInStock)
                Console.WriteLine($"El producto {product} no se encuentra en stock para el usuario {userId}");
        }

        public async Task<BaseResponse<TOrderFulfill>> FulfillOrder(int userId, int storeId, string orderId)
        {
            var orderResponse = new BaseResponse<TOrderFulfill>(ResponseCode.ERROR, "", new TOrderFulfill());

            //Buscar el accessToken de ese usuario
            var accessTokenResponse = await _accessTokenService.GetAccessTokenByUserIdCacheAsync(userId);
            if(accessTokenResponse.res != ResponseCode.OK)
                return orderResponse;

            
            orderResponse = await _tiendanubeService.FullfillOrderAsync(storeId, orderId, accessTokenResponse.token.AccessToken);

            return orderResponse;
        }
    }
}
