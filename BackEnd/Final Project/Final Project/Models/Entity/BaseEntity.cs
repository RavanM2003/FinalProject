namespace Final_Project.Models.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime? UpdatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string AddedBy { get; set; } = "System";
        public bool IsDeleted { get; set; }
    }
}
