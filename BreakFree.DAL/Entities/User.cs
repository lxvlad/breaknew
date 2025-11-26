using System.ComponentModel.DataAnnotations.Schema;


namespace BreakFree.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;
    }
}
