﻿using System.ComponentModel.DataAnnotations;

namespace BlogEngineSGC.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = false;

        [Required]
        public string UserName { get; set; } = string.Empty;
    }
}
