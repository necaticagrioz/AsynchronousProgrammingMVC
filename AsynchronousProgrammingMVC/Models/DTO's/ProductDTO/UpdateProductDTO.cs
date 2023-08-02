using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;

namespace AsynchronousProgrammingMVC.Models.DTO_s.ProductDTO
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur!")]
        public string Description { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [Column(TypeName = "decimal(7,2)")]
        public decimal UnitPrice { get; set; }

        public IFormFile? UploadImage { get; set; }

        [Required(ErrorMessage = "Bir kategori seçiniz!!")]
        public int CategoryId { get; set; }

        public List<Category>? Categories  { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
