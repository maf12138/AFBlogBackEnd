using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
