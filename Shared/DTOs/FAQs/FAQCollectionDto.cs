namespace Shared.DTOs.FAQs
{
    public class FAQCollectionDto
    {
        public IEnumerable<BaseFAQSimpleDto> SiteFAQs { get; set; } = new List<BaseFAQSimpleDto>();
        public IEnumerable<BaseFAQSimpleDto> ProductFAQs { get; set; } = new List<BaseFAQSimpleDto>();
    }
}
