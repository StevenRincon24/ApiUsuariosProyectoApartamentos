using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace apiUsuarios.Models
{
    public class pagos
    {
        [Key]
        public int? id_Pago { get; set; }
        public int? valor_Pagado { get; set; }
        public String? fecha { get; set; }
        public int? id_Obligacion { get; set; }
        public String? id_propiedad { get; set; }


    }
}
