using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Shared.DataTransferObjects.Authentfication
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Old password is a required field.")]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "New password repeat password is a required field.")]
        public string NewPasswordRepeat { get; set; } = string.Empty;
    }
}
