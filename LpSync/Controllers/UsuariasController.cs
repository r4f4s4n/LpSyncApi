using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LpSync.Models;

namespace LpSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariasController : ControllerBase
    {
        private readonly LpSyncContext _context;

        public UsuariasController(LpSyncContext context)
        {
            _context = context;
        }

        // GET: api/Usuarias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuaria>>> GetUsuaria()
        {
            return await _context.Usuaria.Include(x => x.Accesos).ToListAsync();
        }

        // GET: api/Usuarias/rdsm87
        [HttpGet("{username}")]
        public async Task<ActionResult<Usuaria>> GetUsuaria(string username)
        {
            var usuaria = await _context.Usuaria.Include(x => x.Accesos).FirstOrDefaultAsync(i => i.DiscogsUsername == username);

            if (usuaria == null)
            {
                return NotFound();
            }

            return usuaria;
        }

        // PUT: api/usuarias/rdsm87
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUsuaria(string username, Usuaria usuaria)
        {
            if (username != usuaria.DiscogsUsername)
            {
                return BadRequest();
            }

            _context.Entry(usuaria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariaExists(username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarias
        [HttpPost]
        public async Task<ActionResult<Usuaria>> PostUsuaria(Usuaria usuaria)
        {
            _context.Usuaria.Add(usuaria);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsuariaExists(usuaria.DiscogsUsername))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUsuaria", new { id = usuaria.DiscogsUsername }, usuaria);
        }

        // DELETE: api/Usuarias/rdsm87
        [HttpDelete("{username}")]
        public async Task<ActionResult<Usuaria>> DeleteUsuaria(string username)
        {
            var usuaria = await _context.Usuaria.FindAsync(username);
            if (usuaria == null)
            {
                return NotFound();
            }

            _context.Usuaria.Remove(usuaria);
            await _context.SaveChangesAsync();

            return usuaria;
        }

        private bool UsuariaExists(string username)
        {
            return _context.Usuaria.Any(e => e.DiscogsUsername == username);
        }
    }
}
