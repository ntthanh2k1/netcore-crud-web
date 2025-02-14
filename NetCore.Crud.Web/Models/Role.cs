using Microsoft.AspNetCore.Identity;

namespace NetCore.Crud.Web.Models
{
    public class Role : IdentityRole<int>
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
