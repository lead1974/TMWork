using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMWork.Data.Models.Team
{
    public class Mission
    {
        [Key]
        [Required]
        public int MissionId { get; set; }

        [Display(Name = "Our Mission")]
        [Required]
        [DataType(DataType.Text)]
        public string OurMission { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
