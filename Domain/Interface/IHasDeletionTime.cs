namespace Domain.Interface
{
    internal interface IHasDeletionTime
    {
        public DateTime? DeleteTime { get; }
    }
}