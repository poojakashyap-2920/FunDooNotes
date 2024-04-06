using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
   public class UserEntity
    {
       

        // [RegularExpression(@"^[a-zA-Z]{2,}$", ErrorMessage ="Letter must start with Upper")]
        public string FirstName { get; set; }

        // [RegularExpression(@"^[A-Za-z]{3,}$", ErrorMessage = "Letter must start with Upper")]
        public string LastName { get; set; }
      
        // [RegularExpression(@"^[a-z]{5,10}@gmail\.com$", ErrorMessage = "Invalid email format")]
        public string EmailId { get; set; }


        //   [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{8,}$", ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character, and be at least 8 characters long")]
        public string Password { get; set; }
    }
}
