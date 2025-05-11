namespace Coordinator.Models
{
    public record Node(string name)
    {
        public Guid Id { get;set; }
        public ICollection<NodeState>NodeStates { get; set; }
    }
}
