using Auth.Domain.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Models;

public class UserLogin : IdentityUserLogin<int>, IAuditable
{
    public virtual User? User { get; set; }


    // Auditable

    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public int? CreatedById { get; set; }
    public User? CreatedBy { get; set; }
    public int? UpdatedById { get; set; }
    public User? UpdatedBy { get; set; }
}
