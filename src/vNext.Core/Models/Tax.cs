using System;

namespace vNext.Core.Models
{
    public class Tax
    {        
        public int TaxId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal MaxTax { get; set; }
        public string Jurisdiction { get; set; }
        public string Reference { get; set; }
        public bool IsTaxOnCost { get; set; }
        public bool IsExempt { get; set; }
        public int? TaxOnCostTaxId { get; set; }        
        public int Sort { get; set; }
        public int NoteId { get; set; }
       

    }
}
