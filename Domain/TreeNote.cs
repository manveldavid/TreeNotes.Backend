namespace Domain
{
    public class TreeNote
    {
        public Guid Id { get; set; }
        public Guid Parent { get; set; }
        public Guid Creator { get; set; }
        public Guid User { get; set; }
        public DateTime Creation { get; set; }
        public DateTime LastEdit { get; set; }
        public double Weight { get; set; }
        public int Number { get; set; }
        public bool Check { get; set; }
        public bool Share { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
