namespace Management.Models
{
    public class Task: UserActivity
    {
        public int Id { get; set; }
        public string TaskNo { get; set; }
        public string Name { get; set; }
        public string ToDo { get; set; }
        public string FullTask => $"{Id}{Name}{ToDo}";
        public int Critical { get; set; }
        public DateTime DateTime { get; set; }
    }
}
