using System;

namespace vNext.Core.Interfaces
{
    public interface IAuthenticatedRequest
    {
        int UserId { get; set; }
        DateTime CurrentDateTime { get; set; }
        string CustomerKey { get; set; }
    }
}
