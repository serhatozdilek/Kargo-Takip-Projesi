using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KargoTakibi.Models
{
    public class Kargo
    {
        [Key]
        public int KargoID { get; set; }

        [Required(ErrorMessage = "Gönderici adı zorunludur")]
        [Display(Name = "Gönderici Adı")]
        public string GondericiAd { get; set; }

        [Required(ErrorMessage = "Gönderici ili zorunludur")]
        [Display(Name = "Gönderici İli")]
        public string GondericiIl { get; set; }

        [Required(ErrorMessage = "Gönderici ilçesi zorunludur")]
        [Display(Name = "Gönderici İlçesi")]
        public string GondericiIlce { get; set; }

        [Required(ErrorMessage = "Gönderici adresi zorunludur")]
        [Display(Name = "Gönderici Adresi")]
        public string GondericiAdresi { get; set; }

        [Required(ErrorMessage = "Gönderici telefonu zorunludur")]
        [Display(Name = "Gönderici Telefonu")]
        public string GondericiTelefonu { get; set; }

        [Required(ErrorMessage = "Alıcı adı zorunludur")]
        [Display(Name = "Alıcı Adı")]
        public string AliciAd { get; set; }

        [Required(ErrorMessage = "Alıcı ili zorunludur")]
        [Display(Name = "Alıcı İli")]
        public string AliciIl { get; set; }

        [Required(ErrorMessage = "Alıcı ilçesi zorunludur")]
        [Display(Name = "Alıcı İlçesi")]
        public string AliciIlce { get; set; }

        [Required(ErrorMessage = "Alıcı adresi zorunludur")]
        [Display(Name = "Alıcı Adresi")]
        public string AliciAdresi { get; set; }

        [Required(ErrorMessage = "Alıcı telefonu zorunludur")]
        [Display(Name = "Alıcı Telefonu")]
        public string AliciTelefonu { get; set; }

        [Display(Name = "Durum")]
        public string? Durum { get; set; }

        [ForeignKey("Kullanici")]
        public int? KullaniciID { get; set; }
        public virtual Kullanici? Kullanici { get; set; }

        public virtual ICollection<Hareket>? Hareketler { get; set; }
    }
}
