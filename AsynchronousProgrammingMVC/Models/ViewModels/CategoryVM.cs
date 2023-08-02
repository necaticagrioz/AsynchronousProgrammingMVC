using AsynchronousProgrammingMVC.Models.Entities.Abstract;

namespace AsynchronousProgrammingMVC.Models.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
