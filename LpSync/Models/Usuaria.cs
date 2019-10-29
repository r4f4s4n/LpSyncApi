using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LpSync.Models 
{
    public class Usuaria 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string DiscogsUsername { get; set; }

        public bool EsHabilitada { get; set; }

        public ICollection<Acceso> Accesos { get; set; }
    }
}