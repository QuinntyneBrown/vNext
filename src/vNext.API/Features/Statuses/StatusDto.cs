using vNext.Core.Models;

namespace vNext.API.Features.Statuses
{
    public class StatusDto
    {
        public int StatusId { get; set; }
        public string Category { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }

        public static StatusDto FromStatus(dynamic status)
            => new StatusDto
            {
                StatusId = status.StatusId,
                Category = status.Category,
                Status = status.Status,
                Description = status.Description,
                Sort = status.Sort
            };
    }
}
