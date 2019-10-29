using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LpSync.Models 
{
    public class Acceso 
    {
        [Key]
        [Column("AccesoID")]
        public string AccesoID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public Usuaria Usuaria { get; set; }
    }
}