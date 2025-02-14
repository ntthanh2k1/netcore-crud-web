using System.ComponentModel.DataAnnotations;

namespace NetCore.Crud.Web.Dtos.RoleDtos
{
    public class UpdateRoleDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
