using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace TMWork.GoogleReCaptcha.Web.Validation
{
    public abstract class GoogleReCaptchaModelBase
    {
        [Required]
        [GoogleReCaptchaValidation(ErrorMessage = "Are you a robot?")]
        [BindProperty(Name = "g-recaptcha-response")]
        public String GoogleReCaptchaResponse { get; set; }
    }
}
