using vNext.Core.Models;

namespace vNext.API.Features.PropertyValues
{
    public class PropertyValueDto
    {        
        public int PropertyValueId { get; set; }
        public string Code { get; set; }

        public static PropertyValueDto FromPropertyValue(dynamic propertyValue)
            => new PropertyValueDto
            {
                PropertyValueId = propertyValue.PropertyValueId,
                Code = propertyValue.Code
            };
    }
}
