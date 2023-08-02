using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AsynchronousProgrammingMVC.Models.DTO_s.CategoryDTO
{
    public class UpdateCategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur!!!")]
        [MaxLength(60, ErrorMessage = "60 karakter sınırını geçtiniz!!")]
        [MinLength(3, ErrorMessage = "En az 3 karakter girmelisiniz!!")]
        [DisplayName("Kategori Adı")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
