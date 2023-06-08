﻿using EmployeeTaxCalculation.Data.Enums;
using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class TaxDetailsMapper
    {
        public static TaxDetailsDTO Map(TaxDetails taxDetails)
        {
            return new TaxDetailsDTO
            {
                Id = taxDetails.Id,
                EmployeeId = taxDetails.EmployeeId,
                FinancialYearId = taxDetails.FinancialYearId,
                RegimeType = (RegimeTypeEnum)taxDetails.RegimeType,
                TaxPaid = taxDetails.TaxPaid
            };
        }
    }
}
