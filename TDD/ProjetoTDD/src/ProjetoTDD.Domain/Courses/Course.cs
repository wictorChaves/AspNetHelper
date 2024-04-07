namespace ProjetoTDD.Domain.Courses
{
    public class Course
    {
        public Course(string name, int workload, string targetAudience, decimal value)
        {
            this.Name = name;
            this.Workload = workload;
            this.TargetAudience = targetAudience;
            this.Value = value;
        }
        public string Name { get; set; }
        public int Workload { get; set; }
        public string TargetAudience { get; set; }
        public decimal Value { get; set; }

    }
}
