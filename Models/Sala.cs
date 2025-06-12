using System.ComponentModel.DataAnnotations;

namespace PNT1_TP_Cine.Models
{
    public class Sala
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de sala es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El número de sala debe estar entre 1 y 100.")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, 500, ErrorMessage = "La capacidad debe estar entre 1 y 500 asientos.")]
        public int Capacidad { get; set; }


    }
}
