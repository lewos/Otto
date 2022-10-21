using Microsoft.AspNetCore.Mvc;
using Otto.orders.DTOs;
using Otto.orders.Mapper;
using Otto.orders.Models;
using Otto.orders.Services;
using System.Text.Json;

namespace Otto.orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MOrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly QueueTasks _queueTasks;
        private readonly MOrdersService _mOrdersService;

        public MOrdersController(IHttpContextAccessor httpContextAccessor, QueueTasks queueTasks,
            MOrdersService mOrdersService)
        {
            _httpContextAccessor = httpContextAccessor;
            _queueTasks = queueTasks;
            _mOrdersService = mOrdersService;

            //Check for unread notifications and queue if necesary
            // TODO descomentar
            //CheckUnreadNotifications();
        }

        private async void CheckUnreadNotifications()
        {
            var unreadNotifications = await _mOrdersService.GetMUnreadNotificationsAsync();

            var messages = MissedFeedsMapper.GetListMOrderNotificationDTO(unreadNotifications?.Messages);

            foreach (var msj in messages) 
            {
                if(!string.IsNullOrEmpty(msj.Topic) && msj.Topic.Contains("orders_v2"))
                    _queueTasks.Enqueue(_mOrdersService.ProcesarOrden(msj));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(MOrderNotificationDTO dto)
        {
            string jsonString = JsonSerializer.Serialize(dto);

            //Procesar dentro de una cola -- para responder dentro de los 500ms 
           _queueTasks.Enqueue(_mOrdersService.ProcesarOrden(dto));

            return Ok();
        }

        //tengo que tomar el codigo de la url
        //llamar a la api para guardar el token
        //lo tengo que redigir a la pagina 
        //TODO falta la url del front y las paginas de exito y error


        //TODO si la respuesta fue ok, hacer la redireccion a una pagina de exito(devolviendo el mUserId),
        //  Este dato es necesario para que el front actualize el usuario(le tiene que pegar servicio de user)
        //sino a una de error


        // GET api/<OrdersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            string code = Helper.GetCodeFromRequest(request);
            if (!string.IsNullOrEmpty(code)) 
            {
                var tuple = await _mOrdersService.CreateNewMTokenRegisterAsync(code);
                var token = tuple.Item1;
                var res = tuple.Item2;

                if (res.Contains("Ok"))
                    return Redirect($"https://google.com/{token.MUserId}");
            }
            return Redirect("https://yahoo.com");
        }
    }
}
