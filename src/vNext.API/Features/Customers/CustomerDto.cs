namespace vNext.API.Features.Customers
{
    public class CustomerDto
    {        
        public int CustomerId { get; set; }
        public string Code { get; set; }

        public static CustomerDto FromCustomer(dynamic customer)
            => new CustomerDto
            {
                CustomerId = customer.CustomerId,
                Code = customer.Code
            };
    }
}
