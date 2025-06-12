using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PNT1_TP_Cine.Models
{
    public class Ticket
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de ticket es obligatorio.")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "La fecha de compra es obligatoria.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El número de asientos es obligatorio.")]
        [Range(1, 10, ErrorMessage = "El número de asientos debe estar entre 1 y 10.")]
        public int NumeroAsientos { get; set; }

        [ForeignKey("Funcion")]
        public int FuncionId { get; set; }
        public Funcion Funcion { get; set; } = null!;

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

      





    }


}
