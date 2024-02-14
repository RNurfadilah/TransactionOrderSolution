using System.ComponentModel.DataAnnotations;

namespace TransactionOrderAPI.Data
{
    public class UserLogin
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
    }
}