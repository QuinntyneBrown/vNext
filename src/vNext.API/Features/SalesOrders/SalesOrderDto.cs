using vNext.Core.Models;

namespace vNext.API.Features.SalesOrders
{
    public class SalesOrderDto
    {        
        public int SalesOrderId { get; set; }
        public string Code { get; set; }

        public static SalesOrderDto FromSalesOrder(dynamic salesOrder)
            => new SalesOrderDto
            {
                SalesOrderId = salesOrder.SalesOrderId,
                Code = salesOrder.Code
            };
    }
}
