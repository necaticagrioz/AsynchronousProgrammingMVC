using AsynchronousProgrammingMVC.Models.Entities.Abstract;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsynchronousProgrammingMVC.Models.DTO_s.ProductDTO
{
    public class AddProductDTO
    {
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Bu alan zorunludur!")]
        public string Description { get; set; }
        
        public string? Image { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur!")]
        [Column(TypeName = "decimal(7,2)")]
        public decimal UnitPrice { get; set; }

        public IFormFile UploadImage { get; set; }

        [Required(ErrorMessage = "Bir kategori seçiniz!!")]
        public int CategoryId { get; set; }
    }
}
