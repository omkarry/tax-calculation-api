﻿namespace EmployeeTaxCalculation.Data.Models
{
    public class ApiResponse<T>
    {
        public string? Message { get; set; }
        public T? Result { get; set; }
    }
}
