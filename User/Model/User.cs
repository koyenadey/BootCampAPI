using System.ComponentModel.DataAnnotations;

namespace RegisteredUser.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
