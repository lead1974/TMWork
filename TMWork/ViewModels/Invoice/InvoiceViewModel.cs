using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMWork.ViewModels.Invoice
{
    public class KendoTreeViewViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool HasChildren { get; set; }
    }

}
