using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMWork.ViewModels.Invoice
{
    public class InvoiceViewModel
    {
        [Key]
        [Required]
        public int InvoiceId { get; set; }

        [Display(Name = "Invoice Date")]
        [Required]
        [DataType(DataType.Text)]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Invoice Name")]
        [Required]
        [DataType(DataType.Text)]
        public string InvoiceName { get; set; }

        [Display(Name = "Customer Name")]
        [Required]
        [DataType(DataType.Text)]
        public string CustomerName { get; set; }

        [Display(Name = "Phone")]
        [Required]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Display(Name = "Address")]
        [Required]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Display(Name = "Customer BlackListed")]
        public string CustomerBlackListed { get; set; }

        [Display(Name = "Work Notes")]
        [Required]
        [DataType(DataType.Text)]
        public string WorkNotes { get; set; }

        [Display(Name = "Charged Amount")]
        [Required]
        [DataType(DataType.Currency)]
        public float? ChargedAmount { get; set; }

        [Display(Name = "Tax Amount")]
        [DataType(DataType.Currency)]
        public float? TaxAmount { get; set; }

        [Display(Name = "Discount Applied")]
        [DataType(DataType.Currency)]
        public float? DiscountApplied { get; set; }

        [Display(Name = "Parts Notes")]
        [DataType(DataType.Text)]
        public string PartNotes { get; set; }

        [Display(Name = "Parts Paid")]
        [DataType(DataType.Currency)]
        public float? PartsPaid { get; set; }

        [Display(Name = "Warranty From Date")]
        [DataType(DataType.Date)]
        public DateTime WarrantyFromDate { get; set; }

        [Display(Name = "Warranty To Date")]
        [DataType(DataType.Date)]
        public DateTime WarrantyToDate { get; set; }

        [Display(Name = "Payment Type")]
        [DataType(DataType.Text)]
        public string PaymentType { get; set; }

        [Display(Name = "Has Files")]
        [DataType(DataType.Text)]
        public string HasFiles { get; set; }

    }
}
