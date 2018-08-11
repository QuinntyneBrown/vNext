using System;
using vNext.Core.Interfaces;

namespace vNext.Core.Common
{
    public class AuthenticatedRequest : IAuthenticatedRequest
    {
        public int UserId { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public string CustomerKey { get; set; }
    }
}
