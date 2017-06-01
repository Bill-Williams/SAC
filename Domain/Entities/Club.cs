using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Club : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string CityStateZip { get; set; }

        [MaxLength(250)]
        public string Contact { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid Phone number")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public string Email { get; set; }

        [DataType(DataType.Url)]
        [Url(ErrorMessage = "Please enter a valid URL")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true)]
        public string Website { get; set; }

        [MaxLength(2000)]
        public string Directions { get; set; }

        public string IconFileName { get; set; }
    }
}
