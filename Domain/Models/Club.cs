using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Club : BaseEntity
    {
        public Club()
        {
            Users = new HashSet<AspNetUser>();
        }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Club Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [MaxLength(100)]
        [Display(Name = "City, State & ZIP")]

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

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
