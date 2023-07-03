using MediatR;

namespace Domain.Extensions
{
    /// <summary>
    /// 领域事件是由聚合根进行管理的
    /// 供聚合根进行事件注册的接口
    /// </summary>
    public interface IDomainEvents
    {
        IEnumerable<INotification> GetDomainEvents();//获取注册的领域事件
        void AddDomainEvent(INotification notification);//注册领域事件

        void AddDomainEventIfAbsent(INotification notification);//如果领域事件不存在则注册

        void ClearDomainEvents();//移除所有领域事件
    }
}
