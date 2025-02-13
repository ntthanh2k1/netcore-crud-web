using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Crud.Web.Dtos.AuthDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }
}
