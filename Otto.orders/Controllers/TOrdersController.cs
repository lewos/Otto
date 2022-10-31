using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otto.orders.DTOs;
using Otto.orders.Models;
using Otto.orders.Services;
using System.Text.Json;

namespace Otto.orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TOrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly QueueTasks _queueTasks;
        private readonly TOrdersService _tOrdersService;

        public TOrdersController(IHttpContextAccessor httpContextAccessor, QueueTasks queueTasks, TOrdersService tOrdersService)
        {
            _httpContextAccessor = httpContextAccessor;
            _queueTasks = queueTasks;
            _tOrdersService = tOrdersService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(TOrderNotificationDTO dto)
        {

            //var request = _httpContextAccessor.HttpContext.Request;
            //using (var content = new StreamContent(request.Body))
            //{
            //    var contentString = await content.ReadAsStringAsync();
            //}

            //string jsonString = JsonSerializer.Serialize(dto);
            //Console.WriteLine(jsonString);

            _queueTasks.Enqueue(_tOrdersService.ProcesarOrden(dto));
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            string code = Helper.GetCodeFromRequest(request);
            if (!string.IsNullOrEmpty(code))
            {
                var tuple = await _tOrdersService.CreateNewTTokenRegisterAsync(code);
                var token = tuple.Item1;
                var res = tuple.Item2;

                // crear el webhook para obtener las ordenes
                var resCreateOrderWebHook = await _tOrdersService.CreateOrderWebHook(token);


                if (res.Contains("Ok") && resCreateOrderWebHook)
                    return Redirect($"http://localhost:3000/cliente_fulfillment/permisos/permisos_otorgados_tiendanube/#{token.TUserId}");
            }
            return Redirect("http://localhost:3000/cliente_fulfillment/permisos/error_permisos");
        }
    }
}
