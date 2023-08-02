namespace AsynchronousProgrammingMVC.Models.Entities.Abstract
{
    public enum Status { Active = 1, Modified, Passive }
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        private DateTime _createdDate = DateTime.Now;
        public DateTime CreatedDate 
        { 
            get => _createdDate;
            set => _createdDate = value;
        }

        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        private Status _status = Status.Active;
        public Status Status 
        {
            get => _status;
            set => _status = value;
        }
    }
}
