using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMWork.GoogleReCaptcha.Web.Validation;

namespace TMWork.ViewModels.Home
{
    public class ContactViewModel : GoogleReCaptchaModelBase
    {
        [Key]
        [Required]
        public int ContactId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(4096, MinimumLength = 10)]
        public string Message { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
