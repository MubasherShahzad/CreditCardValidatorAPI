using HomeTask.Controllers;
using HomeTask.Models.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HomeTask.Tests.Controllers
{
    public class CardValidationControllerTests
    {
        CardValidationController _controller;

        public CardValidationControllerTests()
        {
            _controller = new CardValidationController();
        }


        // test case if no details of the card is enetered,
        // system should return error as all fields are required
        [Fact]
        public void IsAllCardDetailsAdded_ReturnFalse()
        {
            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "",
                CardNumber = "",
                IssueDate = "",
                CVC = ""
            };

            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Please enter valid CardOwner name.", response.GetValueOrDefault("CardOwner"));
            Assert.Equal("Please enter valid CardNumber.", response.GetValueOrDefault("CardNumber"));
            Assert.Equal("Please enter valid IssueDate.", response.GetValueOrDefault("IssueDate"));
            Assert.Equal("Please enter valid CVC number.", response.GetValueOrDefault("CVC"));
        }


        // check with the correct details if system returns master card
        [Fact]
        public void IsCardDetailsValid_ReturnCardType_Master()
        {
            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "John Doe",
                CardNumber = "5555555555554444",
                IssueDate = "12/2020",
                CVC = "111"
            };

            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Your card is MasterCard.", response.GetValueOrDefault("CardType"));
        }


        // check with the correct details if system returns visa card
        [Fact]
        public void IsCardDetailsValid_ReturnCardType_Visa()
        {
            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "John Doe",
                CardNumber = "4012888888881881",
                IssueDate = "12/2020",
                CVC = "111"
            };

            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Your card is Visa.", response.GetValueOrDefault("CardType"));
        }


        // check with the correct details if system returns american express card
        [Fact]
        public void IsCardDetailsValid_ReturnCardType_AmericanExpress()
        {
            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "John Doe",
                CardNumber = "378282246310005",
                IssueDate = "12/2020",
                CVC = "1111"
            };

            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Your card is AmericanExpress.", response.GetValueOrDefault("CardType"));
        }


        // check the data of american express card
        // but with wrong length of CVC number 3,
        // actual lenght is 4 for American express
        [Fact]
        public void IsCVCValidFor_AmericanExpress_ReturnFalse()
        {
            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "John Doe",
                CardNumber = "378282246310005",
                IssueDate = "12/2020",
                CVC = "111" // American Express has 4 digit CVC valid code
            };

            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Please enter valid CVC number.", response.GetValueOrDefault("CVC"));
        }

        // validate card if the owner name is missing,
        // system showed the respective error
        [Fact]
        public void IsEmptyCardOwnerVerified_ReturnFalse()
        {
            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "",
                CardNumber = "4012888888881881",
                IssueDate = "12/2020",
                CVC = "111"
            };

            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Please enter valid CardOwner name.", response.GetValueOrDefault("CardOwner"));
        }

        // using the expired card from year 2011
        // system should not validate the card
        [Fact]
        public void IsCardExpired_ReturnTrue()
        {
            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "John Doe",
                CardNumber = "5105105105105100",
                IssueDate = "12/2011",
                CVC = "111"
            };

            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Your card is expired.", response.GetValueOrDefault("IssueDate"));
        }

        // trying unusual date format
        // system shoudl convert to C# dateformat to verify
        [Fact]
        public void IsDateFormatValid_ReturnTrue()
        {
            // Only allowed date fornats from regex include 
            // 12/2024
            // 12/24
            // 122024
            // 1224
            // but it will also convert every dateformat supported by C#

            // Arrange
            Dictionary<string, string> response = new Dictionary<string, string>();

            CreditCardModel creditCardModel = new CreditCardModel()
            {
                CardOwner = "John Doe",
                CardNumber = "5105105105105100",
                IssueDate = "12January2021",
                CVC = "111"
            };


            // Act
            response = _controller.ValidateCard(creditCardModel);

            // Assert
            Assert.Equal("Your card is MasterCard.", response.GetValueOrDefault("CardType"));
        }

    }
}
