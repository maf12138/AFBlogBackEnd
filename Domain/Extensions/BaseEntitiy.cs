using MediatR;

namespace Domain.Extensions
{
    /// <summary>
    /// 为了简化实体类的编写
    /// 实现IDomianEvents接口的抽象实体类
    /// </summary>
    public abstract class BaseEntitiy : IDomainEvents
    {

        private List<INotification> DomainEvents = new();

        public void AddDomainEvent(INotification eventItem)
        {
            DomainEvents.Add(eventItem);
        }

        public void AddDomainEventIfAbsent(INotification eventItem)
        {
            if (!DomainEvents.Contains(eventItem))
            {
                DomainEvents.Add(eventItem);
            }
        }
        public void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }

        public IEnumerable<INotification> GetDomainEvents()
        {
            return DomainEvents;
        }
    }
}
