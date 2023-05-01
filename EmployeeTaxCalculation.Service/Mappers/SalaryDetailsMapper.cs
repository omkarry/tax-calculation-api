using EmployeeTaxCalculation.Data.Models;
using EmployeeTaxCalculation.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaxCalculation.Service.Mappers
{
    public class SalaryDetailsMapper
    {
        public static SalaryDetailsDto Map(SalaryDetails entity)
        {
            return new SalaryDetailsDto
            {
                Id = entity.Id,
                BasicPay = entity.BasicPay,
                HRA = entity.HRA,
                ConveyanceAllowance = entity.ConveyanceAllowance,
                MedicalAllowance = entity.MedicalAllowance,
                OtherAllowance = entity.OtherAllowance,
                EPF = entity.EPF,
                ProfessionalTax = entity.ProfessionalTax,
                EmployeeId = entity.EmployeeId
            };
        }

    }
}
