using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiUsuarios.Models
{
    [Table("propietario")]
    public class Persona
    {
        public String? tipo_identificacion { get; set; }
        [Key]
        public String numero_identificacion { get; set; }
        public String? nombre { get; set; }

        public String? apellido { get; set; }
        public String? correo { get; set; }
        public String? telefono { get; set;}
        public String? contrasenhia { get; set; }


    }
}
