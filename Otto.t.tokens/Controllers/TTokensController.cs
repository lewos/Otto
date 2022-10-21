using Microsoft.AspNetCore.Mvc;
using Otto.models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Otto.t.tokens.Controllers
{
    public class TTokensController : ControllerBase
    {
        private readonly OttoDbContext _context;
        //private readonly MTokenService _service;

        public TTokensController(OttoDbContext context)
        {
            _context = context;
            //_service = service;

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTOrderCommand command)
        {
            string jsonString = JsonSerializer.Serialize(command);

            Console.WriteLine(jsonString);

            return Ok(jsonString);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok("ok?");
        }
    }

    public class CreateTOrderCommand
    {
        [JsonPropertyName("store_id")]
        public long StoreId { get; set; }
        [JsonPropertyName("event")]
        public string Event { get; set; }
        [JsonPropertyName("id")]
        public long Id { get; set; }


        //{
        //    'store_id':876,
        //    'event':'product/created',
        //    'id':48708
        //}
    }
}
