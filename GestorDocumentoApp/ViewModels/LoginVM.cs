using System.ComponentModel.DataAnnotations;

namespace GestorDocumentoApp.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password es requerido")]
        [DataType(DataType.Password,ErrorMessage ="Contrasena no valida")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
