using System;
using System.Collections.Generic;

namespace LpSync.Dto
{
    public class LpDto
    {
        // id 
        public int Id { get; set; }
        // basic_information -> master_id 
        public int IdMaestra { get; set; }
        // date_added 
        public DateTime FechaRegistro { get; set; }
        // basic_information -> title 
        public string Titulo { get; set; }
        // basic_information -> year
        public int AnoEdicion { get; set; }
        public string PortadaUrl { get; set; }
        // basic_information -> artists  
        public List<ArtistaDto> Artistas { get; set; }
        // basic_information -> labels  
        public List<SelloDto> Sellos { get; set; }

        // release_id 
        //...

        // master_id 
        //...
    }
}
