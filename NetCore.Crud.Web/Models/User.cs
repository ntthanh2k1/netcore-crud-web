using Microsoft.AspNetCore.Identity;

namespace NetCore.Crud.Web.Models
{
    public class User : IdentityUser<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<UserAssignment>? UsersAssignments { get; set; }
    }
}
