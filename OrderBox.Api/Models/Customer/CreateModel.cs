using System.ComponentModel.DataAnnotations;

namespace Orderbox.Api.Models.Customer
{
    public class CreateModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
