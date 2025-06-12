using System.ComponentModel.DataAnnotations;

namespace PNT1_TP_Cine.Models
{
    public class Genero
    {

        [Key]
      
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del género es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del género no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
