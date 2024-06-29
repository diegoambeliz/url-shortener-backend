using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Dto
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(50)]
        public string username { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string pwd { get; set; } = string.Empty;
    }
}
