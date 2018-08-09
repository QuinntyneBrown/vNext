using vNext.Core.Models;

namespace vNext.API.Features.PaymentTerms
{
    public class PaymentTermDto
    {        
        public int PaymentTermId { get; set; }
        public string Code { get; set; }

        public static PaymentTermDto FromPaymentTerm(dynamic paymentTerm)
            => new PaymentTermDto
            {
                PaymentTermId = paymentTerm.PaymentTermId,
                Code = paymentTerm.Code
            };
    }
}
