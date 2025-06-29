using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PNT1_TP_Cine.Models
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La sinopsis es obligatoria.")]
        public string Sinopsis { get; set; } = string.Empty;

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(1, 300, ErrorMessage = "La duración debe estar entre 1 y 300 minutos.")]
        public int Duracion { get; set; }

        [Required(ErrorMessage = "La fecha de estreno es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaEstreno { get; set; }

        [Required(ErrorMessage = "La imagen es obligatoria.")]
        [DataType(DataType.ImageUrl)]
        [Url]
        public string Imagen { get; set; } = string.Empty;


        [ForeignKey("Genero")]
        public int GeneroId { get; set; }

        public Genero Genero { get; set; } = null!;

    }
}
