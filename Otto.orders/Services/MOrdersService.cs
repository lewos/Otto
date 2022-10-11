using Otto.models;
using Otto.models.Responses;
using Otto.orders.DTOs;
using Otto.orders.Mapper;

namespace Otto.orders.Services
{
    public class MOrdersService
    {
        private readonly AccessTokenService _accessTokenService;
        private readonly MercadolibreService _mercadolibreService;
        private readonly OrderService _orderService;
        private readonly UserService _userService;
        private readonly StockService _stockService;

        public MOrdersService(AccessTokenService accessTokenService, MercadolibreService mercadolibreService, OrderService orderService, UserService userService, StockService stockService)
        {
            _accessTokenService = accessTokenService;
            _mercadolibreService = mercadolibreService;
            _orderService = orderService;
            _userService = userService;
            _stockService = stockService;
        }
        public async Task<int> ProcesarOrden(MOrderNotificationDTO dto)
        {
            //Ver si el topico es el que necesito
            if (!string.IsNullOrEmpty(dto.Topic) && dto.Topic.Contains("orders_v2"))
            {
                // TODO Guardar en un cache en memoria, algunas ordenes asi no consulto la base constantemente
                if (await isNewOrder(dto))
                {
                    return await CreateOrder(dto);
                }
                else
                {
                    return await UpdateOrder(dto);
                }
                return 0;
            }
            return 0;
        }
        private async Task<int> CreateOrder(MOrderNotificationDTO dto)
        {
            var mOrder = await GetMOrder(dto);
            if (mOrder != null)
            {
                //Ver como llega la orden. LLega un item/producto por orden, por lo tanto tengo que guardar la orden y se va a agrupar por el pack id
                //Si no tiene pack_id, la orden contiene un solo producto
                var a = await CreateOrder(mOrder);
                return a;
            }
            else
            {
                //No obtuve la orden, mercadolibre va a mandar otra notificacion o
                // voy a leer cuando reinicie la aplicacion las notificaciones no leidas
                Console.WriteLine($"No se pudo obtener la orden");
                return 0;
            }

        //TODO Ver si el producto o item de la orden esta dentro del deposito o es una venta/orden que no esta en el deposito            
        }
        private async Task<int> UpdateOrder(MOrderNotificationDTO dto)
        {
            var mOrder = await GetMOrder(dto);
            if (mOrder != null)
            {
                var a = await UpdateOrderTable(mOrder);
                return a;
            }
            else
            {
                //No obtuve la orden, mercadolibre va a mandar otra notificacion o
                // voy a leer cuando reinicie la aplicacion las notificaciones no leidas
                Console.WriteLine($"No se pudo obtener la orden");
                return 0;
            }
        }
        private async Task<int> UpdateOrderTable(MOrderDTO order)
        {

            //string cadena = "{\"id\": 2000004068397970,\"date_created\": \"2022-08-18T19:15:12-03:00\",\"date_closed\": \"2022-08-18T19:15:33-03:00\",\"last_updated\": \"2022-08-19T16:42:03-03:00\",\"manufacturing_ending_date\": null,\"comment\": null,\"pack_id\": 2000003758168231,\"pickup_id\": null,\"order_request\": {\"return\": null,\"change\": null},\"fulfilled\": false,\"mediations\": [{\"id\": 5142295289}],\"total_amount\": 1578,\"paid_amount\": 0,\"coupon\": {\"id\": null,\"amount\": 0},\"expiration_date\": \"2022-09-15T19:15:33-03:00\",\"order_items\": [{\"item\": {\"id\": \"MLA1153822235\",\"title\": \"Item De Prueba - Por Favor, No Ofertar\",\"category_id\": \"MLA380657\",\"variation_id\": null,\"seller_custom_field\": null,\"variation_attributes\": [],\"warranty\": \"Garantía de fábrica: 8 meses\",\"condition\": \"new\",\"seller_sku\": \"IDPPNOR\",\"global_price\": null,\"net_weight\": null},\"quantity\": 2,\"requested_quantity\": {\"value\": 2,\"measure\": \"unit\"},\"picked_quantity\": null,\"unit_price\": 789,\"full_unit_price\": 789,\"currency_id\": \"ARS\",\"manufacturing_days\": null,\"sale_fee\": 368.54,\"listing_type_id\": \"gold_pro\"}],\"currency_id\": \"ARS\",\"payments\": [{\"id\": 25004539421,\"order_id\": 2000004068397970,\"payer_id\": 1164373159,\"collector\": {\"id\": 1164363887},\"card_id\": null,\"site_id\": \"MLA\",\"reason\": \"Item De Prueba - Por Favor, No Ofertar\",\"payment_method_id\": \"account_money\",\"currency_id\": \"ARS\",\"installments\": 1,\"issuer_id\": null,\"atm_transfer_reference\": {\"company_id\": null,\"transaction_id\": null},\"coupon_id\": null,\"activation_uri\": null,\"operation_type\": \"regular_payment\",\"payment_type\": \"account_money\",\"available_actions\": [\"\"],\"status\": \"refunded\",\"status_code\": null,\"status_detail\": \"bpp_refunded\",\"transaction_amount\": 1578,\"transaction_amount_refunded\": 1578,\"taxes_amount\": 0,\"shipping_cost\": 0,\"coupon_amount\": 0,\"overpaid_amount\": 0,\"total_paid_amount\": 1578,\"installment_amount\": null,\"deferred_period\": null,\"date_approved\": \"2022-08-18T19:15:33-03:00\",\"authorization_code\": null,\"transaction_order_id\": null,\"date_created\": \"2022-08-18T19:15:33-03:00\",\"date_last_modified\": \"2022-08-19T16:41:59-03:00\"}],\"shipping\": {\"id\": 41596080714},\"status\": \"cancelled\",\"status_detail\": null,\"tags\": [\"test_order\",\"pack_order\",\"not_delivered\",\"not_paid\"],\"feedback\": {\"buyer\": null,\"seller\": null},\"context\": {\"channel\": \"marketplace\",\"site\": \"MLA\",\"flows\": []},\"buyer\": {\"id\": 1164373159,\"nickname\": \"TESTJPME7DLO\",\"first_name\": \"Test\",\"last_name\": \"Test\",\"phone\": null,\"alternative_phone\": null,\"email\": null},\"seller\": {\"id\": 1164363887,\"nickname\": \"TESTC5MGH6XY\",\"first_name\": \"Test\",\"last_name\": \"Test\",\"phone\": {\"extension\": \"\",\"area_code\": \"01\",\"number\": \"1111-1111\",\"verified\": false},\"alternative_phone\": {\"area_code\": \"\",\"extension\": \"\",\"number\": \"\"},\"email\": \"test_user_47518799@testuser.com\"},\"taxes\": {\"amount\": null,\"currency_id\": null,\"id\": null}}";
            //var objetooooo= JsonSerializer.Deserialize<MOrderDTO>(cadena);
            //Console.WriteLine(objetooooo);

            // Es una orden que tiene un nuevo claim o mediacion // ej cancelada
            //Si se cancela la compra
            var state = OrderState.Pendiente;
            if (order.Status.Contains("cancelled"))
                state = OrderState.Cancelada;

            // Buscar ese usuario id
            var user = await GetUser(order.Seller.Id.ToString());

            var newOrder = new Order
            {
                UserId = user.Id,
                MUserId = order.Seller.Id,
                MOrderId = order.Id,
                MShippingId = order.Shipping.Id,
                //BusinessId
                ItemId = order.OrderItems[0].Item.Id,
                ItemDescription = order.OrderItems[0].Item.Title,
                Quantity = order.OrderItems[0].Quantity,
                PackId = order.PackId.ToString(),
                SKU = order.OrderItems[0].Item.SellerSku,
                State = state,
                //ShippingStatus = state,
            };

            if (state == OrderState.Cancelada)
                newOrder.StateDescription = $"La orden fue cancelada por el comprador. Id de mediacion {order.Mediations.FirstOrDefault().Id}";

            var algo = await _orderService.UpdateOrderTableByMIdAsync((long)newOrder.MOrderId, newOrder);
            Console.WriteLine($"Cantidad de filas afectadas {algo.Item2}");
            return algo.Item2;
        }
        private async Task<MOrderDTO> GetMOrder(MOrderNotificationDTO dto)
        {
            //Buscar el accessToken de ese usuario
            Token accessToken = await GetAccessToken(dto);

            var orderResponse = new MOrderResponse<MOrderDTO>(ResponseCode.ERROR, "", new MOrderDTO());

            if (accessToken != null)
            {
                orderResponse = await _mercadolibreService.GetMOrderAsync((long)dto.MUserId, dto.Resource, accessToken.AccessToken);
            }

            return orderResponse.res == ResponseCode.OK
                ? orderResponse.mOrder
                : null;

        }
        private async Task<User> GetUser(string MUserId)
        {
            var userResponse = await _userService.GetUserByMIdCacheAsync(MUserId);
            return userResponse.res == ResponseCode.OK
                ? userResponse.user
                : null;

        }
        private async Task<int> GetProductInStockByMItemId(string mItemId, int userId)
        {
            var tupleStockResponse = await _stockService.GetProductInStockByMItemId(mItemId, userId);
            if (tupleStockResponse.Item1)
                return tupleStockResponse.Item2;
            return 0;
        }

        private async Task<bool> isNewOrder(MOrderNotificationDTO dto)
        {
            try
            {
                var resource = long.Parse(dto.Resource.Split("/")[2]);

                var order = await _orderService.GetByMOrderIdWithoutPackIdAsync(resource);

                //si la orden es null, no lo encontro
                return order == null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<bool> isNewOrder(long resource)
        {
            try
            {
                var order = await _orderService.GetByMOrderIdWithoutPackIdAsync(resource);

                //si la orden es null, no lo encontro
                return order == null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<int> CreateOrder(MOrderDTO order)
        {
            // Buscar ese usuario id
            var user = await GetUser(order.Seller.Id.ToString());

            //Buscar ese producto en el stock
            int productInStockId = await GetProductInStockByMItemId(order?.OrderItems[0].Item.Id, user.Id);

            //El producto no esta en stock
            if (productInStockId == 0) 
            {
                Console.WriteLine($"El producto {order?.OrderItems[0].Item.Id} no se encuentra en stock para el usuario {user?.Id}");
                return 0;
            }

            var utcNow = DateTime.UtcNow;
            var newOrder = new Order
            {
                UserId = user?.Id == null ? 0 : user.Id,
                MUserId = order?.Seller?.Id == null ? null : order.Seller.Id,
                MOrderId = order?.Id == null ? null : order.Id,
                MShippingId = order.Shipping?.Id == null ? null : order.Shipping.Id,
                CompanyId = user?.CompanyId == null ? null : user.CompanyId,                
                ProductInStockId = productInStockId == 0? null : productInStockId,
                ItemId = order?.OrderItems[0].Item.Id == null ? null : order.OrderItems[0].Item.Id,
                ItemDescription = order?.OrderItems[0].Item.Title == null ? null : order.OrderItems[0].Item.Title,
                Quantity = order.OrderItems[0].Quantity,
                PackId = order.PackId == null ? "Otto-M-" + Guid.NewGuid().ToString("n") : order.PackId.ToString(),
                SKU = order.OrderItems[0].Item.SellerSku == null ? null : order.OrderItems[0].Item.SellerSku,
                State = OrderState.Pendiente,
                ShippingStatus = State.Pendiente,
                Created = utcNow,
                Modified = utcNow,
                InProgress = false,
            };

            //Ver que no exista una orden con esos datos para no duplicar
            if (await isNewOrder(order.Id))
            {
                var algo = await _orderService.CreateAsync(newOrder);
                Console.WriteLine($"Cantidad de filas afectadas {algo.Item2}");
                return algo.Item2;
            }
            else
            {
                return 0;
            }
        }

        private async Task<Token> GetAccessToken(MOrderNotificationDTO dto)
        {
            var res = await _accessTokenService.GetTokenCacheAsync((long)dto.MUserId);

            if (hasTokenExpired(res.token))
                res = await _accessTokenService.GetTokenAfterRefresh((long)dto.MUserId);
            return res.token;
        }
        private async Task<Token> GetAccessTokenByMuserId(long mUserId)
        {
            var res = await _accessTokenService.GetTokenCacheAsync(mUserId);

            if (hasTokenExpired(res.token))
                res = await _accessTokenService.GetTokenAfterRefresh(mUserId);
            return res.token;
        }
        private bool hasTokenExpired(Token token)
        {
            var utcNow = DateTime.UtcNow;
            // Si expiro o si esta a punto de expirar
            if (token.ExpiresAt < utcNow + TimeSpan.FromMinutes(10))
                return true;
            return false;
        }
        public async Task<MissedFeedsDTO> GetMUnreadNotificationsAsync()
        {
            //este el es id del usario dueño de la aplicacion en mercadolibre
            long mUserId = long.Parse(Environment.GetEnvironmentVariable("APP_MUSER_ID_OWNER"));
            string appId = Environment.GetEnvironmentVariable("APP_ID");

            //Buscar el accessToken de ese usuario
            Token accessToken = await GetAccessTokenByMuserId(mUserId);

            var mUnreadNotificationsResponse = new MUnreadNotificationsResponse<MissedFeedsDTO>(ResponseCode.ERROR, "", new MissedFeedsDTO());

            if (accessToken != null)
            {
                mUnreadNotificationsResponse = await _mercadolibreService.GetUnreadNotificationsAsync(mUserId, appId, accessToken.AccessToken);
            }

            return mUnreadNotificationsResponse.res == ResponseCode.OK
                ? mUnreadNotificationsResponse.missedFeeds
                : null;
        }

        public async Task<Tuple<Token, string>> CreateNewMTokenRegisterAsync(string code)
        {
            MCodeForTokenDTO mCodeForTokenDTO = await _mercadolibreService.GetTokenWithCodeAsync(code);

            //var mToken = AccessTokenMapper.GetMTokenDTO(mCodeForTokenDTO);
            var token = AccessTokenMapper.GetToken(mCodeForTokenDTO);

            var mAccessTokenResponse = await _accessTokenService.CreateNewRegisterAsync(token, "Mercadolibre");
            if (mAccessTokenResponse.res == ResponseCode.OK)
                return new Tuple<Token, string>(mAccessTokenResponse.token, "Ok");
            else
                return new Tuple<Token, string>(null, "Error");
        }
    }
}
