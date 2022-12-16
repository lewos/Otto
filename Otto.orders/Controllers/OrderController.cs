using Microsoft.AspNetCore.Mvc;
using Otto.models;
using Otto.models.Responses;
using Otto.orders.DTOs;
using Otto.orders.Services;
using System.Net;

namespace Otto.orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly StockService _stockService;
        private readonly AccessTokenService _accessTokenService;
        private readonly MercadolibreService _mercadolibreService;
        private readonly MOrdersService _mOrdersService;
        private readonly TOrdersService _tOrdersService;

        public OrderController(OrderService orderService,StockService stockService, AccessTokenService accessTokenService,
            MercadolibreService mercadolibreService, MOrdersService mOrdersService, TOrdersService tOrdersService)
        {
            _orderService = orderService;
            _stockService = stockService;
            _accessTokenService = accessTokenService;
            _mercadolibreService = mercadolibreService;
            _mOrdersService = mOrdersService;
            _tOrdersService = tOrdersService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _orderService.GetAsync();
            return Ok(result);
        }

        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetOrdersByCompanyId(int id)
        {
            var result = await _orderService.GetOrdersAsync(id);
            return Ok(result);
        }

        [HttpGet("company/{id}/{state}")]
        public async Task<IActionResult> GetOrdersByCompanyIdAndState(int id, string state)
        {
            if (Enum.TryParse<OrderState>(state, true, out OrderState newState))
            {
                var result = await _orderService.GetOrdersAsync(id, newState);
                return Ok(result);
            }
            else
            {
                var posibleStates = Enum.GetValues(typeof(OrderState)).Cast<OrderState>()
                        .Select(d => (d, (int)d))
                        .ToList();

                return NotFound($"Los estados posibles son: {String.Join(',', posibleStates)}");
            }
        }

        [HttpGet("company/{id}/pack/{pack}")]
        public async Task<IActionResult> GetOrdersByCompanyIdAndPakcId(int id, string pack)
        {
            var result = await _orderService.GetOrderByPackIdAsync(id, pack);
            return Ok(result);
        }

        [HttpPut("company/{id}/pack/{pack}/start")]
        public async Task<IActionResult> TakeOrderByPackId([FromBody] InProgressDTO dto, int id, string pack)
        {
            var result = await _orderService.UpdateOrderInProgressByPackIdAsync(id, pack, dto.UserIdInProgress);
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict(result.Item3);
        }

        [HttpPut("company/{id}/pack/{pack}/stop")]
        public async Task<IActionResult> StopTakingOrderByPackId([FromBody] InProgressDTO dto, int id, string pack)
        {
            var result = await _orderService.UpdateOrderStopInProgressByPackIdAsync(id, pack, dto.UserIdInProgress);
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict(result.Item3);
        }

        [HttpPut("company/{id}/pack/{pack}/end")]
        public async Task<IActionResult> FinalizeOrderByPackId([FromBody] InProgressDTO dto, int id, string pack)
        {
            var result = await _orderService.UpdateFinalizeOrderByPackIdAsync(id, pack, dto.UserIdInProgress);
            if (result.Item2 > 0)
            {
                var itemsInPack = result.Item1;

                var tupleResultProduct = await _stockService.GetProductInStockById((int)itemsInPack.Items.FirstOrDefault().ProductInStockId);

                bool pudoActualizarStock = false;
                bool pudoCumplirLaOrden = false;

                if (tupleResultProduct.Item1 && tupleResultProduct.Item2.Origin == "Mercadolibre")
                {
                    foreach (var orderDTO in itemsInPack.Items)
                    {
                        // restar stock de la venta finalizada ej "MLA1149237532"
                        pudoActualizarStock = await _stockService.UpdateQuantityByMItemId(orderDTO.Quantity, orderDTO.ItemId, (int)orderDTO.UserId, (int)orderDTO.CompanyId);
                    }
                }
                else if (tupleResultProduct.Item1 && tupleResultProduct.Item2.Origin == "Tiendanube")
                {
                    foreach (var orderDTO in itemsInPack.Items)
                    {
                        // restar stock de la venta finalizada ej "MLA1149237532"
                        pudoActualizarStock = await _stockService.UpdateQuantityByTItemId(orderDTO.Quantity, orderDTO.ItemId, (int)orderDTO.UserId, (int)orderDTO.CompanyId);
                    }
                    //TODO Cambiar el estado de la orden
                    var pudoCumplirLaOrdenResponse = await _tOrdersService.FulfillOrder((int)itemsInPack.Items[0].UserId, (int)itemsInPack.Items[0].TUserId, itemsInPack.PackId);
                    pudoCumplirLaOrden = pudoCumplirLaOrdenResponse.res == ResponseCode.OK; 

                }
                if (pudoActualizarStock && tupleResultProduct.Item2.Origin == "Mercadolibre")
                    return Ok("Ok");
                else if (pudoActualizarStock && tupleResultProduct.Item2.Origin == "Tiendanube" && pudoCumplirLaOrden)
                    return Ok("Ok");
                else if (pudoActualizarStock && tupleResultProduct.Item2.Origin == "Tiendanube" && !pudoCumplirLaOrden)
                    return Conflict("No se pudo actualizar el estado de al orden en el canal de venta. Verificar");
                else
                    return Conflict("No se pudo actualizar la cantidad de del producto en el inventario. Verificar");
            }
            return Conflict(result.Item3);
        }

        [HttpGet("company/{id}/pack/{pack}/print/{userInProgress}")]
        public async Task<IActionResult> PrintOrderReceiptByPackId(int id, string pack, int userInProgress)
        {
            var packDto = await _orderService.GetOrderInProgressByPackIdAsync(pack, userInProgress);
            if (packDto?.Items?.Count <= 0)
                return Conflict("No se encontro una orden con ese id");

            var orderDto = packDto.Items.FirstOrDefault();

            var tupleResultProduct = await _stockService.GetProductInStockById((int)orderDto.ProductInStockId);
            if (tupleResultProduct.Item1 && tupleResultProduct.Item2.Origin == "Mercadolibre")
            {
                //Token accessToken = await _accessTokenService.GetAccessTokenByUserIdCacheAsync((int)orderDto.UserId);
                var accessTokenResponse = await _accessTokenService.GetAccessTokenByUserIdCacheAsync((int)orderDto.UserId);

                if (hasTokenExpired(accessTokenResponse.token))
                    accessTokenResponse = await _accessTokenService.GetTokenAfterRefresh((long)accessTokenResponse.token.MUserId);

                var orderResponse = new MOrderResponse<MOrderDTO>(ResponseCode.ERROR, "", new MOrderDTO());

                if (accessTokenResponse.token != null)
                {
                    //obtener el link del pdf para imprimir
                    var pdf = await _mercadolibreService.GetPrintOrderAsync((long)orderDto.MShippingId, accessTokenResponse.token.AccessToken);

                    return Ok(pdf);

                }
                return Conflict("Error al obtener el token");

            }
            else if (tupleResultProduct.Item1 && tupleResultProduct.Item2.Origin == "Tiendanube")
            {
                //TODO
                return Ok("ver imprimir tienda nube");
            }
            else
                return Conflict("No se encontro el producto dentro del inventario");
        }

        [HttpGet("company/{id}/pack/{pack}/reprint/{userInProgress}")]
        public async Task<IActionResult> RePrintOrderReceiptByPackId(int id, string pack, int userInProgress)
        {
            var packDto = await _orderService.GetOrderByPackIdAsync(pack, userInProgress);
            if (packDto?.Items?.Count <= 0)
                return Conflict("No se encontro una orden con ese id");

            var orderDto = packDto.Items.FirstOrDefault();

            var tupleResultProduct = await _stockService.GetProductInStockById((int)orderDto.ProductInStockId);
            if (tupleResultProduct.Item1 && tupleResultProduct.Item2.Origin == "Mercadolibre")
            {
                //Token accessToken = await _accessTokenService.GetAccessTokenByUserIdCacheAsync((int)orderDto.UserId);
                var accessTokenResponse = await _accessTokenService.GetAccessTokenByUserIdCacheAsync((int)orderDto.UserId);

                if (hasTokenExpired(accessTokenResponse.token))
                    accessTokenResponse = await _accessTokenService.GetTokenAfterRefresh((long)accessTokenResponse.token.MUserId);

                var orderResponse = new MOrderResponse<MOrderDTO>(ResponseCode.ERROR, "", new MOrderDTO());

                if (accessTokenResponse.token != null)
                {
                    //obtener el link del pdf para imprimir
                    var pdf = await _mercadolibreService.GetPrintOrderAsync((long)orderDto.MShippingId, accessTokenResponse.token.AccessToken);

                    return Ok(pdf);

                }
                return Conflict("Error al obtener el token");

            }
            else if (tupleResultProduct.Item1 && tupleResultProduct.Item2.Origin == "Tiendanube")
            {
                //TODO
                return Ok("ver imprimir tienda nube");
            }
            else
                return Conflict("No se encontro el producto dentro del inventario");
        }


        //[HttpGet("company/{id}/pack/{pack}/print/{userInProgress}")]
        //public async Task<HttpResponseMessage> PrintOrderReceiptByPackIdPrueba(int id, string pack, int userInProgress)
        //{
        //    var packDto = await _orderService.GetOrderInProgressByPackIdAsync(pack, userInProgress);

        //    var orderDto = packDto.Items.FirstOrDefault();

        //    var tupleResultProduct = await _stockService.GetProductInStockById((int)orderDto.ProductInStockId);
        //    if (tupleResultProduct.Item1 && tupleResultProduct.Item2.Origin == "Mercadolibre")
        //    {
        //        Token accessToken = await _accessTokenService.GetAccessTokenByUserIdCacheAsync((int)orderDto.UserId);
        //        var accessTokenResponse = await _accessTokenService.GetAccessTokenByUserIdCacheAsync((int)orderDto.UserId);

        //        if (hasTokenExpired(accessTokenResponse.token))
        //            accessTokenResponse = await _accessTokenService.GetTokenAfterRefresh((long)accessTokenResponse.token.MUserId);

        //        var orderResponse = new MOrderResponse<MOrderDTO>(ResponseCode.ERROR, "", new MOrderDTO());

        //        if (accessTokenResponse.token != null)
        //        {
        //            obtener el link del pdf para imprimir
        //            var pdf = await _mercadolibreService.GetPrintOrderAsync((long)orderDto.MShippingId, accessTokenResponse.token.AccessToken);


        //            return pdf;                    
        //        }
        //    }
        //    HttpResponseMessage response2 = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    return response2;
        //}



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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            var result = await _orderService.CreateAsync(order);
            return Created("GetOrder", result.Item1);
        }       

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Order order)
        {
            order.Id = id;
            var result = await _orderService.UpdateOrderTableByIdAsync(id, order);
            return Ok(result.Item1);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            var result = await _orderService.DeleteAsync(id, order);
            return Ok(result);
        }
    }
}
