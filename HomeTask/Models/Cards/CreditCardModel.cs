using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeTask.Models.Cards
{
    public class CreditCardModel
    {
        /*
            These Validations are added manuualy to the function 
            but the same can be achieved by the annotation tags here
        */

        // validation for checking full name 
        // there sould be atleast 1 character at start, then optional space,
        // it can be repeated at least 1 time and at most 3 times 
        // no numbers are allowed with the names
        //[Required]
        //[RegularExpression(@"^((?:[A-Za-z]+ ?){1,3})$", ErrorMessage = "Please enter your full name as on the card")]
        public string CardOwner { get; set; }

        //[Required]
        public string CardNumber { get; set; }

        // datatype can be 'DateTime' considering the validation required for input type
        // Validation for valid date formats
        // Valid formats with the regex include
        // 12/2024
        // 12/24
        // 122024
        // 1224
        //[Required]
        //[RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$", ErrorMessage = "Date format is incorrect")]
        public string IssueDate { get; set; }

        // datatype can be 'int' considering the validation added to input field
        // validation for 3 or 4 characters long int        
        //[Required]
        public string CVC { get; set; }
    }
}
