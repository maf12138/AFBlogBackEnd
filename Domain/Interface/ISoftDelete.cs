namespace Domain.Interface
{
    internal interface ISoftDelete
    {
        bool IsDeleted { get; }
        void SoftDelete();
    }
}