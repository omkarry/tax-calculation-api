namespace EmployeeTaxCalculation.Constants
{
    public static class ResponseMessages
    {
        public const string WrongCredentials = "Wrong Credentials";
        public const string LoginSuccessful = "Login Successful";
        public const string DataFormat = "Data should be correct format";
        
        public const string UserExistWithEmail = "User already exist with this email";
        public const string UserNotExistWithEmail = "User not exist with this email";
        public const string UserExistWithUsername = "User already exist with this username";
        public const string UserNotExistWithUsername= "User not exist with this username";

        public const string AdminList = "List of admins";
        public const string AdminNotFound = "Admin not found";
        public const string AdminDetails = "Admin details";
        public const string AdminRegistered = "Admin registered successfully";
        public const string AdminNotCreated = "Unable to create admin";
        public const string AdminUpdated = "Admin updated successfully";
        public const string AdminNotUpdated= "Unable to update admin";
        public const string AdminDeleted = "Admin deleted successfully";
        public const string AdminNotDeleted = "Unable to delete admin";

        public const string EmployeeList = "List of employees";
        public const string NoEmployees = "No employees found";

        public const string TaxDetailsList = "List of employees tax details";
        public const string UnableToGetTaxDetails = "Unable to get tax details";
        public const string EmployeeTaxDetails = "Employee tax details";

        public const string Count = "Count of employees, pending declaration and pending salary details";
    }
}
