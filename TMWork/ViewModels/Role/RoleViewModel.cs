using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TMWork.Data.Models.User;

namespace TMWork.ViewModels.Role
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {

        }

    }

    public class RoleIndex
    {
        public IEnumerable<AuthRole> Roles { get; set; }
    }

    public class RoleNew
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }

    public class RoleEdit
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }

}
