using Domain.Entities;
using Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrasturacture
{
    /// <summary>
    /// 可以直接用identityDbContext但是用UserManager等简化了identityDbContext操作
    /// 引入mediatR
    /// 可将事件逻辑抽象到BaseDbContext中
    /// 这里没有拆分成小上下文，并且继承的是Identity框架的上下文
    /// 发布事件是在调用领域模型类的方法时发布，在控制器中事件定义处理者
    /// </summary>
    public class BlogDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly IMediator _mediator;
        public DbSet<Article> Articles { get; private set; }
        public DbSet<Comment> Comments { get; private set; }
        public DbSet<Category> Categories { get; private set; }
        public DbSet<Tag> Tags { get; set; }

        public BlogDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges(bool acceptAllChangeOnSuccess)
        {
            //强制使项目不能用同步保存方法
            throw new NotImplementedException("Don't call SaveChanges with synchronous(同步的) method. Use the other one.");
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangeOnSuccess, CancellationToken cancellationToken = default)
        {
            //ChangeTracker对象是上下文中用来对实体类的变化进行追踪的对象,
            //Entries<IDomainEvents>获得的是所有实现了IDomain接口的追踪实体类
            var domianEntities = this.ChangeTracker.Entries<IDomainEvents>()
                .Where(x => x.Entity.GetDomainEvents().Any()); 
            var domainEvents = domianEntities.SelectMany(x => x.Entity.GetDomainEvents()).ToList();
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);//在上下文保存修改时自动发布领域事件
            }
            return await base.SaveChangesAsync(acceptAllChangeOnSuccess, cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            //显示更详细的异常信息
            optionsBuilder.EnableDetailedErrors();
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 官方写法,得一个个来modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArticleConfig).Assembly);
            //表示加载当前程序集中所有实现了IEntityTypeConfiguration接口的类。
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        }
    }
}

