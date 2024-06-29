using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Dto
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string username { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string first_name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string last_name { get; set; } = string.Empty;
        [Required]
        [MaxLength(150)]
        public string email { get; set; } = string.Empty;
        [Required]
        [MaxLength(30)]
        public string pwd { get; set; } = string.Empty;
    }
}
