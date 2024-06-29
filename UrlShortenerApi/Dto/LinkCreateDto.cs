using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Dto
{
    public class LinkCreateDto
    {
        [Required]
        [MaxLength(250)]
        public string description { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string link { get; set; } = string.Empty;
    }
}
