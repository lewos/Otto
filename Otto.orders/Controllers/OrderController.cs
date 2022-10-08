using Microsoft.AspNetCore.Mvc;
using Otto.models;
using Otto.orders.DTOs;
using Otto.orders.Services;

namespace Otto.orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly MOrdersService _mOrdersService;

        public OrderController(OrderService orderService, MOrdersService mOrdersService)
        {
            _orderService = orderService;
            _mOrdersService = mOrdersService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _orderService.GetAsync();
            return Ok(result);
        }

        [HttpGet("GetPendingOrders")]
        public async Task<IActionResult> GetPendingOrders()
        {
            var result = await _orderService.GetPendingAsync();
            return Ok(result);
        }


        [HttpGet("GetOrderByMOrderId/{id}", Name = "GetOrderByMOrderId")]
        public async Task<IActionResult> GetOrderByMOrderId(string id)
        {
            var result = await _orderService.GetByMOrderIdAsync(id);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet("GetOrderByPackId/{id}", Name = "GetOrderByPackId")]
        public async Task<IActionResult> GetOrderByPackId(string id)
        {
            var result = await _orderService.GetOrderByPackId(id);
            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            var result = await _orderService.CreateAsync(order);
            return Created("GetOrder", result.Item1);
        }

        [HttpPut("TakeOrderByMOrderId/{id}")]
        public async Task<IActionResult> TakeOrderByMOrderId(string id, [FromBody] InProgressDTO dto)
        {
            dto.Id = id;
            var result = await _orderService.UpdateOrderInProgressByMOrderIdAsync(id, dto.UserIdInProgress);
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict("No se encontro una orden con ese id o la misma ya se encuentra tomada por otro operario");
        }

        [HttpPut("TakeOrderByPackId/{id}")]
        public async Task<IActionResult> TakeOrderByPackId(string id, [FromBody] InProgressDTO dto)
        {
            dto.Id = id;
            var result = await _orderService.UpdateOrderInProgressByPackIdAsync(id, dto.UserIdInProgress);
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict("No se encontro una orden con ese id o la misma ya se encuentra tomada por otro operario");
        }

        [HttpPut("StopTakingOrderByMOrderId/{id}")]
        public async Task<IActionResult> StopTakingOrderByMOrderId(string id, [FromBody] InProgressDTO dto)
        {
            dto.Id = id;
            var result = await _orderService.UpdateOrderStopInProgressByMOrderIdAsync(id, dto.UserIdInProgress);
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict("No se encontro una orden con ese id o el el id del operario no es el mismo que la tomo o la misma ya se encuentra en un estado final");
        }


        [HttpPut("StopTakingOrderByPackId/{id}")]
        public async Task<IActionResult> StopTakingOrderByPackId(string id, [FromBody] InProgressDTO dto)
        {
            dto.Id = id;
            var result = await _orderService.UpdateOrderStopInProgressByPackIdAsync(id, dto.UserIdInProgress);
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict("No se encontro una orden con ese id o el el id del operario no es el mismo que la tomo o la misma ya se encuentra en un estado final");
        }


        [HttpPut("FinalizeOrderByMOrderId/{id}")]
        public async Task<IActionResult> FinalizeOrderByMOrderId(string id, [FromBody] InProgressDTO dto)
        {
            dto.Id = id;
            var result = await _mOrdersService.UpdateFinalizeOrderByMOrderIdAsync(id, dto.UserIdInProgress);
            
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict("No se encontro una orden con ese id o el el id del operario no es el mismo que la tomo o la misma ya se encuentra en un estado final");
        }


        [HttpPut("FinalizeOrderByPackId/{id}")]
        public async Task<IActionResult> FinalizeOrderByPackId(string id, [FromBody] InProgressDTO dto)
        {
            dto.Id = id;
            var result = await _mOrdersService.UpdateFinalizeOrderByPackIdAsync(id, dto.UserIdInProgress);            
            if (result.Item2 > 0)
                return Ok(result.Item1);
            else
                return Conflict("No se encontro una orden con ese id o el el id del operario no es el mismo que la tomo o la misma ya se encuentra en un estado final");
        }

        [HttpPost("PrintOrderReceiptByMOrderId/{id}")]
        public async Task<IActionResult> PrintOrderReceiptByMOrderId(string id, [FromBody] PrintReceiptOrderDTO dto)
        {
            dto.Id = id;

            var result = await _mOrdersService.GetPrintOrderAsync(id, dto);

            return Ok(result);            
        }

        [HttpPost("PrintOrderReceiptByPackId/{id}")]
        public async Task<IActionResult> PrintOrderReceiptByPackId(string id, [FromBody] PrintReceiptOrderDTO dto)
        {
            dto.Id = id;

            var result = await _mOrdersService.GetPrintOrderByPackIdAsync(id, dto);

            return Ok(result);
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
