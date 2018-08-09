namespace vNext.Core.Models
{
    public class Concurrency
    {
        public int ConcurrencyId { get; set; }
        public int Id { get; set; }
        public string Domain { get; set; }
        public int Version { get; set; }
    }
}
