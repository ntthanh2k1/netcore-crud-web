using System.ComponentModel.DataAnnotations;

namespace NetCore.Crud.Web.Dtos.RoleDtos
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Password is required.")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
