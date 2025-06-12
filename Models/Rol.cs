using System.ComponentModel.DataAnnotations;

namespace PNT1_TP_Cine.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

    }
}
