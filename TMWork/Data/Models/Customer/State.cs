using System.ComponentModel.DataAnnotations;

namespace TMWork.Data.Models.Customer
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string StateName { get; set; }
    }
}
