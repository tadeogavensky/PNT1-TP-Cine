

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PNT1_TP_Cine.Models
{
    public class Funcion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora de la función son obligatorias.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        [ForeignKey("Pelicula")]
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; } = null!;

        [ForeignKey("Sala")]
        public int SalaId { get; set; }

        public Sala Sala { get; set; } = null!;
    }
}
