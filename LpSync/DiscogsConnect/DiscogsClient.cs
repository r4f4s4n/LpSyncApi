using LpSync.Dto;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LpSync.DiscogsConnect
{
    public class DiscogsClient
    {
        // Propiedades de la clase
        private string ApiUrl { get; set; }
        private string Key { get; set; }
        private string Secret { get; set; }
        private WebClient Client { get; set; }

        /// <summary>
        /// Constructor del cliente.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        public DiscogsClient(string key = "gFJcsQgKsCLieGXQqrPV", string secret = "nBFhVIKPMpKopSgQWXmMbsfxoYZddavv")
        {
            ApiUrl = "https://api.discogs.com";
            Key = key;
            Secret = secret;
        }

        /// <summary>
        /// Consulta un usuario de Discogs con su colección completa de LPs.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns></returns>
        public UsuariaDto GetUsuaria(string username)
        {
            UsuariaDto usuaria = new UsuariaDto();
            try
            {
                JObject userJson = null;
                try
                {
                    userJson = JObject.Parse(GetUserJson(username));
                }
                catch (Exception)
                {
                    //
                    throw;
                }

                string mensaje = (string)userJson["message"];

                if (mensaje != null && mensaje.Equals("User does not exist or may have been deleted."))
                {
                    throw new Exception($"El usuario {username} no existe en Discogs");
                }

                userJson = JObject.Parse(GetUserJson(username));

                usuaria.Username = (string)userJson["username"];
                usuaria.Nombre = (string)userJson["name"];
                usuaria.Localidad = (string)userJson["location"];
                usuaria.NumeroLps = 0;
                try
                {
                    usuaria.NumeroLps = int.Parse((string)userJson["num_collection"]);
                }
                catch (Exception) {}
                usuaria.AvatarUrl = (string)userJson["avatar_url"];

            }
            catch (Exception)
            {
                throw;
            }

            return usuaria;
        }

        /// <summary>
        /// Consulta un usuario de Discogs con su colección completa de LPs.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns></returns>
        public List<LpDto> GetLps(string username)
        {
            List<LpDto> lps = new List<LpDto>();
            try
            {
                JObject collectionsFoldersJson = null;
                try
                {
                    collectionsFoldersJson = JObject.Parse(GetCollectionsJson(username));
                }
                catch (Exception)
                {
                    //
                    throw;
                }

                var foldersJson = collectionsFoldersJson["folders"];
                
                foreach (var folderJson in foldersJson)
                {
                    JObject coleccionJson = null;
                    try
                    {
                        coleccionJson = JObject.Parse(GetLpsJson(username, (string)folderJson["id"]));
                    }
                    catch (Exception)
                    {
                        //
                        throw;
                    }

                    var lpsJson = coleccionJson["releases"];

                    foreach (var lpJson in lpsJson)
                    {
                        LpDto lp = new LpDto();

                        lp.Id = int.Parse((string)lpJson["id"]);
                        lp.IdMaestra = int.Parse((string)lpJson["basic_information"]["master_id"]);
                        try
                        {
                            lp.FechaRegistro = Convert.ToDateTime((string)lpJson["date_added"]);
                        }
                        catch (Exception) { }
                        lp.Titulo = (string)lpJson["basic_information"]["title"];
                        lp.AnoEdicion = int.Parse((string)lpJson["basic_information"]["year"]);

                        try
                        {
                            lp.PortadaUrl = GetLpCoverUrl(lp.Id.ToString());
                        }
                        catch (Exception)
                        {
                            //
                            throw;
                        }

                        List<SelloDto> sellos = new List<SelloDto>();
                        foreach (var selloJson in lpJson["basic_information"]["labels"])
                        {
                            SelloDto sello = new SelloDto();

                            sello.Id = int.Parse((string)selloJson["id"]);
                            sello.Nombre = (string)selloJson["name"];

                            sellos.Add(sello);
                        }
                        lp.Sellos = sellos;

                        List<ArtistaDto> artistas = new List<ArtistaDto>();
                        foreach (var artistaJson in lpJson["basic_information"]["artists"])
                        {
                            ArtistaDto artista = new ArtistaDto();

                            artista.Id = int.Parse((string)artistaJson["id"]);
                            artista.Nombre = (string)artistaJson["name"];

                            artistas.Add(artista);
                        }
                        lp.Artistas = artistas;

                        lps.Add(lp);
                    }


                }

            }
            catch (Exception)
            {
                throw;
            }

            return lps;
        }

        /// <summary>
        /// Consulta si un usuario de Discogs existe.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns></returns>
        public bool IsUserExists(string username)
        {
            string usernameJson = null;
            try
            {
                JObject userJson = JObject.Parse(GetUserJson(username));

                userJson = JObject.Parse(GetUserJson(username));

                usernameJson = (string)userJson["username"];
            }
            catch (Exception)
            {
                throw new Exception($"El usuario {username} no existe en Discogs");
            }

            return usernameJson != null;
        }

        /// <summary>
        /// Consulta un usuario de Discogs.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns></returns>
        public string GetUserJson(string username)
        {
            string userJson;
            using (Client = new WebClient())
            {
                Client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                Client.Headers.Add("Content-Type", "application/json");
                Client.Headers.Add($"Authorization: Discogs key={Key}, secret={Secret}");

                Client.UseDefaultCredentials = true;

                userJson = Client.DownloadString(ApiUrl + "/users/" + username);
            };

            return userJson;
        }

        /// <summary>
        /// Consulta las colecciones de LPs de un usuario de Discogs.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns></returns>
        public string GetCollectionsJson(string username)
        {
            string collectionJson;
            using (Client = new WebClient())
            {
                Client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                Client.Headers.Add("Content-Type", "application/json");
                Client.Headers.Add($"Authorization: Discogs key={Key}, secret={Secret}");

                Client.UseDefaultCredentials = true;

                collectionJson = Client.DownloadString(ApiUrl + "/users/" + username + "/collection/folders");
            };

            return collectionJson;
        }

        /// <summary>
        /// Consulta llos LPs de la carpeta de la colección recibida.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="folderId"Id de la carpeta</param>
        /// <returns></returns>
        public string GetLpsJson(string username, string folderId)
        {
            string lpsJson;
            using (Client = new WebClient())
            {
                Client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                Client.Headers.Add("Content-Type", "application/json");
                Client.Headers.Add($"Authorization: Discogs key={Key}, secret={Secret}");

                Client.UseDefaultCredentials = true;

                lpsJson = Client.DownloadString(ApiUrl + "/users/" + username + "/collection/folders/" + folderId + "/releases");
            };

            return lpsJson;
        }

        /// <summary>
        /// Consulta la imagen de un LP.
        /// </summary>
        /// <param name="idRelease">ID del album</param>
        /// <returns></returns>
        public string GetLpCoverUrl(string idRelease)
        {
            string urlCover;
            using (Client = new WebClient())
            {
                Client.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                Client.Headers.Add("Content-Type", "application/json");
                Client.Headers.Add($"Authorization: Discogs key={Key}, secret={Secret}");

                Client.UseDefaultCredentials = true;

                string release = Client.DownloadString(ApiUrl + "/releases/" + idRelease);

                int startPos = release.IndexOf("images\"") + 19;
                int endPos = release.IndexOf(".jpg", startPos) + 4;

                urlCover = release.Substring(startPos, endPos - startPos);
            };

            return urlCover;
        }
    }
}
