using System;

namespace vNext.Core.Interfaces
{
    public interface ILoggableRequest
    {
        int CreatedByUserId { get; set; }
        DateTime CreatedDateTime { get; set; }
    }
}
