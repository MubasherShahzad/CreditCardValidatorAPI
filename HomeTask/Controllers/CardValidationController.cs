using HomeTask.Models.Cards;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeTask.Controllers
{
    // adding versioning breaks the principle of REST that states that URL of a resource should never change
    // however, it can be added as per requirement 
    // currently APIs have no authentication (and authorization), it can be added with tokens 

    [Route("api/v{version:apiVersion}/creditcard")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CardValidationController : ControllerBase
    {
        /// <summary>
        /// Default starting page of validation
        /// </summary>
        /// <returns></returns>
        [Route("home")]
        public string Welcome()
        {
            return "Welcome to the Credit Card Validation API";
        }


        /// <summary>
        /// this is the main function to validat card data,
        /// if validated, reutrns card type
        /// if not validated, return all possible errors in the data 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("validate")]
        [HttpPost]
        public Dictionary<string, string> ValidateCard([FromBody] CreditCardModel item)
        {
            var errosList = new Dictionary<string, string>();

            #region Validation for Required feilds
            /*
                There are 2 ways of adding validation for reuired,
                1) adding Annotation tags to the Model class (added but commented out there for reference)
                2) adding manually, the same way followed in this region
            */

            Regex cardOwnerRegex = new Regex(@"^((?:[A-Za-z]+ ?){1,3})$");
            Regex issueDateFormatRegex = new Regex(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$");

            // validation for card owner and also checking with regex that it does not contains other card data
            if (string.IsNullOrEmpty(item.CardOwner) && !cardOwnerRegex.IsMatch(item.CardOwner))
            {
                errosList.Add(key: "CardOwner", value: "Please enter valid CardOwner name.");
            }

            // validation for empty card number
            if (string.IsNullOrEmpty(item.CardNumber))
            {
                errosList.Add(key: "CardNumber", value: "Please enter valid CardNumber.");
            }

            // issue date is not empty and valid date format (format generally used for credit cards)
            if (string.IsNullOrEmpty(item.IssueDate) && !issueDateFormatRegex.IsMatch(item.IssueDate))
            {
                errosList.Add(key: "IssueDate", value: "Please enter valid IssueDate.");
            }

            // if cvv is not null
            if (string.IsNullOrEmpty(item.CVC))
            {
                errosList.Add(key: "CVC", value: "Please enter valid CVC number.");
            }

            #endregion


            // if IssueDate is added and valid
            if (!errosList.ContainsKey("IssueDate"))
            {
                try
                {
                    DateTime issueConvertedDate = DateTime.Parse(item.IssueDate);

                    if (!IsCardDateValid(issueConvertedDate))
                    {
                        errosList.Add(key: "IssueDate", value: "Your card is expired.");
                    }
                }
                catch (Exception ex)
                {
                    errosList.Add(key: "IssueDate", value: "Please enter valid IssueDate.");
                }

            }

            // if there are no erros for card number
            if (!errosList.ContainsKey("CardNumber"))
            {
                var checkCardNum = IsCardNumberValid(item.CardNumber);

                // CVC validators for 3 digit (for visa and master) and 4 digits (for american express) 
                Regex threeDigitCVCValidator = new Regex(@"^\d{3}$");
                Regex fourDigitCVCValidator = new Regex(@"^\d{4}$");

                // check for invalid card number
                if (((int)checkCardNum) == 0)
                {
                    errosList.Add(key: "CardNumber", value: "Please enter valid CardNumber.");
                }
                else if (((int)checkCardNum) == 3)  // if card is American Express then CVC should be 4 digits
                {
                    // American express cards have 4 digit cvc number
                    if (!fourDigitCVCValidator.IsMatch(item.CVC))
                    {
                        errosList.Add(key: "CVC", value: "Please enter valid CVC number.");
                    }
                }
                else
                {
                    // visa and master cards have 3 digit cvc number
                    if (!threeDigitCVCValidator.IsMatch(item.CVC))
                    {
                        errosList.Add(key: "CVC", value: "Please enter valid CVC number.");
                    }
                }

                // check if there are no errors, the return the card type
                if (errosList.Count == 0)
                {
                    errosList.Add(key: "CardType", value: "Your card is " + checkCardNum.ToString() + ".");
                }
            }

            return errosList;
        }


        #region Custom Validations


        /// <summary>
        /// Check that card number is valid and should be MasterCard, Visa or AmericanExpress
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static CardType IsCardNumberValid(string cardNumber)
        {
            // regex for master card, visa and american express
            var masterCardRegex = new Regex(@"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$");
            var visaCardRegex = new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$");
            var americanExpressCradRegex = new Regex(@"^3[47][0-9]{13}$");

            if (masterCardRegex.IsMatch(cardNumber))
                return CardType.MasterCard;
            else if (visaCardRegex.IsMatch(cardNumber))
                return CardType.Visa;
            else if (americanExpressCradRegex.IsMatch(cardNumber))
                return CardType.AmericanExpress;
            else
                return CardType.Invalid;
        }


        /// <summary>
        /// Issue date should be before current date
        /// </summary>
        /// <param name="issueDate"></param>
        /// <returns></returns>
        public static bool IsCardDateValid(DateTime issueDate)
        {
            bool result = true;

            // if card issue date is after today's date
            if (DateTime.Now < issueDate)
            {
                result = false;
            }
            else if (DateTime.Now.AddYears(-5) > issueDate)
            {
                // assumption => Card Validity 5 years,
                // checking if card is issued before 5 years
                // this number can be changed as per requirement

                result = false;
            }

            return result;
        }


        #endregion

    }
}
