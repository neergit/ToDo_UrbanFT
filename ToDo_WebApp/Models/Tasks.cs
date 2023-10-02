namespace ToDo_WebApp.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum Status
    {
        Pending,
        InProgress,
        Completed
    }
}

