namespace Management.Models
{
    public enum TaskStatus
    {
        Waiting,
        InProgress,
        Done
    }
    public class Task: UserActivity
    {
        public int Id { get; set; }
        public int TaskNo { get; set; }
        public string Name { get; set; }
        public string ToDo { get; set; }
        public string FullTask => $"{Id}{Name}{ToDo}";
        public int Critical { get; set; }
        public DateTime DateTime { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Waiting;

    }
}
