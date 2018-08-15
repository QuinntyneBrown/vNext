using System;

namespace vNext.Core.Interfaces
{
    public interface IAuthenticatedRequest
    {
        int CurrentUserId { get; set; }
        DateTime CurrentDateTime { get; set; }
        string CustomerKey { get; set; }
    }
}
