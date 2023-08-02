using AsynchronousProgrammingMVC.Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsynchronousProgrammingMVC.Models.Entities.Concrete
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string Image { get; set; }

        //NotMapped => Database'e bu sütunu oluşturma, Sadec C#' da kullanacağım demek.
        [NotMapped]
        public IFormFile UploadImage { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
