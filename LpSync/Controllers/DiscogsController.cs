using LpSync.DiscogsConnect;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LpSync.Controllers
{
    /*[ApiController]
    [Route("api/[controller]")]
    public class DiscogsController : ControllerBase
    {
        // Propiedades de la clase
        private DiscogsClient Discogs { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DiscogsController()
        {
            Discogs = new DiscogsClient(null, null);
        }


        // GET api/discogs/user/{username}
        [HttpGet("user/{username}")]
        public ActionResult<string> GetUsuario(string username)
        {
            try
            {
                return Discogs.GetUser(username);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET api/discogs/collection/{username}
        [HttpGet("collection/{username}")]
        public ActionResult<string> GetColeccion(string username)
        {
            try
            {
                return Discogs.GetUser(username);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET api/discogs/lp-cover/{idRelease}
        [HttpGet("lp-cover/{idRelease}")]
        public ActionResult<string> GetAlbumCover(string idRelease)
        {
            try
            {
                return Discogs.GetLpCover(idRelease);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }*/
}
