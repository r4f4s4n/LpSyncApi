using System.Collections.Generic;

namespace LpSync.Dto
{
    public class UsuariaDto
    {
        // username
        public string Username { get; set; }
        // name
        public string Nombre { get; set; }
        // location
        public string Localidad { get; set; }
        // num_collection
        public int NumeroLps { get; set; }
        // avatar_url
        public string AvatarUrl { get; set; }
        // https://api.discogs.com/users/rdsm87/collection/folders  ->>> Releases
        public List<LpDto> Lps { get; set; }

        public UsuariaDto() { }

        public UsuariaDto(string username, string nombre, string localidad, int numeroLps, string avatarUrl, List<LpDto> lps)
        {
            Username = username;
            Nombre = nombre;
            Localidad = localidad;
            NumeroLps = numeroLps;
            AvatarUrl = avatarUrl;
            Lps = lps;
        }
    }
}
