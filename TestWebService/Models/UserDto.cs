using System.ComponentModel.DataAnnotations;

namespace TestWebService.Models
{
    public class UserDto
    {
        public long Id { get; set; }
        [Required, MaxLength(Consts.STRING_LEN)]
        public string FirstName { get; set; }
        [Required, MaxLength(Consts.STRING_LEN)]
        public string LastName { get; set; }
        [Required, EmailAddress, MaxLength(Consts.EMAIL_LEN)]
        public string Email { get; set; }
    }
}