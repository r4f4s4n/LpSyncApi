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
    public class AccesosController : ControllerBase
    {
        private readonly LpSyncContext _context;

        public AccesosController(LpSyncContext context)
        {
            _context = context;
        }

        // GET: api/Accesos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Acceso>>> GetAcceso()
        {
            return await _context.Acceso.ToListAsync();
        }

        // GET: api/Accesos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Acceso>> GetAcceso(string id)
        {
            var acceso = await _context.Acceso.FindAsync(id);

            if (acceso == null)
            {
                return NotFound();
            }

            return acceso;
        }

        // PUT: api/Accesos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAcceso(string id, Acceso acceso)
        {
            if (id != acceso.AccesoID)
            {
                return BadRequest();
            }

            _context.Entry(acceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccesoExists(id))
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

        // POST: api/Accesos
        [HttpPost]
        public async Task<ActionResult<Acceso>> PostAcceso(Acceso acceso)
        {
            _context.Acceso.Add(acceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAcceso", new { id = acceso.AccesoID }, acceso);
        }

        // DELETE: api/Accesos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Acceso>> DeleteAcceso(string id)
        {
            var acceso = await _context.Acceso.FindAsync(id);
            if (acceso == null)
            {
                return NotFound();
            }

            _context.Acceso.Remove(acceso);
            await _context.SaveChangesAsync();

            return acceso;
        }

        private bool AccesoExists(string id)
        {
            return _context.Acceso.Any(e => e.AccesoID == id);
        }
    }
}
