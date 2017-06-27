using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Contact : BaseEntity
    {
        [MaxLength(250)]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid Phone number")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public string Email { get; set; }

        [Required]
        public Guid ClubId { get; set; }

        public virtual Club Club { get; set; }

    }
}
