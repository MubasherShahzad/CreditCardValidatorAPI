# Credit Card Validator API
This repository contains the code for verification of credit card data through API build using ASP .NET Core 3.1 <br>

HomeTask is the main project with the API. <br>
HomeTask.Tests is the project with xUnit tests. <br>

## API Controller
Controller name is CardValidationController.cs and is under Controllers folder in HomeTask project.
ValidateCard is the major function with added validation and constraints.
<br>

### Validation and Constraints
Few validations and constrainsts are added in the controller but the same can be achieved by adding data-annotation tags in the model class. 
<br>

## Routing
Routing is added for safety reasons on the main controller. <br>
Default versioning value can be modified from the ConfigureServices function in Startup.cs from HomeTask project. <br>

    [Route("api/v{version:apiVersion}/creditcard")]
    [ApiVersion("1.0")]

## Added/Modifed Files
HomeTask => Controllers => CardValidationController.cs (API Controller) <br>
HomeTask => Models => Cards => CardType.cs (ENUM for Card Types) <br>
HomeTask => Models => Cards => CreditCardModel.cs (Model class for credit card data) <br>
HomeTask.Tests => Controllers => CardValidationControllerTests.cs (xUnit Test cases for API controller) <br>

## Request from Postman
    Request Type: POST
    URL: https://localhost:{portnumber}/api/v1.0/creditcard/validate/
    Body (type: JSON):
    {
        "CardOwner": "",
        "CardNumber": "",
        "IssueDate": "",
        "CVC": ""
    }


## Future Work
1) Requirements can be achieved in multiple other ways by adding services and factory layer. It depends on the scope and complexity of the requirement.
2) Test cases can be made more mature if services layer is added and DI can be seen working in that case.
