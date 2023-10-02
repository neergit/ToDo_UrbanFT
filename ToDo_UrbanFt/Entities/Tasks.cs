namespace ToDo_UrbanFt.Entities
{
    public class Tasks
	{
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

