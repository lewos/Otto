using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otto.m.tokens.DTOs;
using Otto.m.tokens.Services;
using Otto.models;

namespace Otto.m.tokens.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MTokensController : ControllerBase
    {
        private readonly OttoDbContext _context;
        private readonly MTokenService _service;

        public MTokensController(OttoDbContext context, MTokenService service)
        {
            _context = context;
            _service = service;

        }

        // GET: api/MTokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Token>>> GetMTokens()
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }

            return await _service.GetAsync();
        }

        // GET: api/MTokens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Token>> GetMToken(long id)
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }
            var mToken = await _context.Tokens.FindAsync(id);

            if (mToken == null)
            {
                return NotFound();
            }

            return mToken;
        }


        // GET: api/MTokens/ByMUserId/5
        [HttpGet("ByMUserId/{id}")]
        public async Task<ActionResult<Token>> GetMTokenByUser(long id)
        {
            var mToken = await _service.GetMTokenByUserAsync(id);

            if (mToken == null)
            {
                return NotFound();
            }

            return mToken;
        }


        // POST: api/MTokens/RefreshByMUserId/5
        [HttpGet("RefreshByMUserId/{id}")]
        public async Task<ActionResult<Token>> RefreshByUser(long id) 
        {
            var mToken = await _service.RefreshMTokenByUserAsync(id);

            if (mToken == null)
            {
                return NotFound();
            }

            return Ok(mToken);
        }


        // PUT: api/MTokens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMToken(long id, Token token)
        {

            if (id != token.Id)
            {
                return BadRequest();
            }

            try
            {
                var rowsAffected = await _service.UpdateAsync(id,token);
                return Update(rowsAffected);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MTokenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private IActionResult Update(int rowsAffected)
        {
            if (rowsAffected > 0)
                return Ok($"{rowsAffected} registros actualizados");
            else
                return Conflict("Error al Update de token");
        }

        // POST: api/MTokens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostMToken(Token requestToken)
        {
            if (_context.Tokens == null)
            {
                return Problem("Entity set 'OttoContext.MTokens'  is null.");
            }

            // Si ya existe un token con ese mismo usuario, hago el update
            var token = await _context.Tokens.Where(t => t.MUserId == requestToken.MUserId).FirstOrDefaultAsync();
            if (token != null && token.MUserId == requestToken.MUserId)
            {
                var tuple = await _service.UpdateWithMTokenIdAsync(requestToken);
                var newDTO = tuple.Item1;
                var rowsAffected = tuple.Item2;
                if (rowsAffected > 0)
                    return Ok(newDTO);
                else
                    return Conflict("Error al Update de token");
            }
            else
            {
                var tuple = await _service.Create(requestToken);
                var newDTO = tuple.Item1;
                return CreatedAtAction("GetMToken", new { id = newDTO.Id }, newDTO);

            }
        }

        // DELETE: api/MTokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMToken(long id)
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }
            var mToken = await _context.Tokens.FindAsync(id);
            if (mToken == null)
            {
                return NotFound();
            }

            _context.Tokens.Remove(mToken);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool MTokenExists(long id)
        {
            return (_context.Tokens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

}
