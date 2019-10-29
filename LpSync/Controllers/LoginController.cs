using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LpSync.Models;
using LpSync.Dto;
using LpSync.DiscogsConnect;
using System.Collections.Generic;

namespace LpSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // Propiedades de la clase
        private DiscogsClient Discogs { get; set; }
        private readonly LpSyncContext _context;

        public LoginController(LpSyncContext context)
        {
            _context = context;

            Discogs = new DiscogsClient();
        }

        // POST: api/login
        [HttpPost]
        public async Task<ActionResult<UsuariaDto>> PostLogin(UsuariaDto usuariaBody)
        {
            UsuariaDto usuariaDto = new UsuariaDto();
            try
            {
                usuariaDto = Discogs.GetUsuaria(usuariaBody.Username);

                // Consulta si la usuaria existe en base de datos.
                Usuaria usuaria = UsuariaExists(usuariaBody.Username);

                // Si la usuaria existe y no está habilitada, aborta la operación
                if (usuaria != null && !usuaria.EsHabilitada)
                {
                    // TODO Informar que no está habilitada

                    return BadRequest("La usuaria con la que intenta acceder no tiene el acceso permitido");
                }

                try
                {
                    // Si la usuaria no existe, la crea
                    if (usuaria == null)
                    {
                        usuaria = new Usuaria
                        {
                            DiscogsUsername = usuariaBody.Username,
                            EsHabilitada = true
                        };

                        _context.Usuaria.Add(usuaria);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (UsuariaExists(usuariaBody.Username) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                try
                {
                    // Acto seguido, haya creado una nueva usuaria o haya entrado con una existente, vincula un nuevo acceso.
                    Acceso acceso = new Acceso
                    {
                        Usuaria = usuaria,
                        Fecha = DateTime.Now
                    };

                    _context.Acceso.Add(acceso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                throw e;
            }


            return usuariaDto;
        }

        // GET: api/login/rdsm87
        [HttpGet("{username}")]
        public async Task<ActionResult<List<LpDto>>> GetLps(string username)
        {
            List<LpDto> lpsDto = null;

            var usuaria = await _context.Usuaria.FirstOrDefaultAsync(i => i.DiscogsUsername == username);

            if (usuaria == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    lpsDto = Discogs.GetLps(username);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("(403) FORBIDDEN"))
                    {
                        return BadRequest("La colección de LPs del usuario es privada, necesita autenticación.<br/>Abordaremos esta opción en futuras actualizaciones.<br/>Gracias por tu comprensión.");
                    }
                    else
                    {
                        return BadRequest(e.Message);
                    }
                    
                }
            }

            return lpsDto;
        }

        private Usuaria UsuariaExists(string username)
        {
            return _context.Usuaria.Where(e => e.DiscogsUsername == username).SingleOrDefault();
        }
    }
}
