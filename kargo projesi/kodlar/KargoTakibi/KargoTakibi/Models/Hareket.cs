using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KargoTakibi.Models
{
    public class Hareket
    {
        [Key]
        public int HareketID { get; set; }

        [ForeignKey("Kargo")]
        public int KargoID { get; set; }

        [Required]
        public DateTime Tarih { get; set; }

        [Required]
        public string Durum { get; set; }

        public virtual Kargo Kargo { get; set; }
    }
}
