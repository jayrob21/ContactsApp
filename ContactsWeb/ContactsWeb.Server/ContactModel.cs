using System.ComponentModel.DataAnnotations;

namespace ContactsWeb.Server
{
    public class ContactModel
    {
        public ContactModel()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }

        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }
        [Required, MaxLength(50), MinLength(2)]
        public string LastName { get; set; }
        [Required, EmailAddress, MaxLength(50)]
        public string Email { get; set; }
    }
}