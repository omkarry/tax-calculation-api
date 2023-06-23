SET IDENTITY_INSERT Sections ON;

INSERT INTO Sections (Id, SectionName) VALUES
	(1, 'Section10'),
	(2, 'Section80C'),
	(3, 'Section80CCD'),
	(4, 'Section80CCG'),
	(5, 'Section80D'),
	(6, 'Section80DD'),
	(7, 'Section80DDB'),
	(8, 'Section80G'),
	(9, 'Section80U');
SET IDENTITY_INSERT Sections OFF;

SET IDENTITY_INSERT SubSections ON;

INSERT INTO SubSections (Id, SubSectionName, SectionId, MaxLimit) VALUES
	(1, 'LeaveTravelAllowance', 1, '20000'),
	(2, 'HouseRentAllowance', 1, NULL),
	(3, 'ProvidentFund', 2, NULL),
	(4, 'LifeInsurance', 2, NULL),
	(5, 'PPF', 2, NULL),
	(6, 'NSC', 2, NULL),
	(7, 'HousingLoan', 2, NULL),
	(8, 'ChildrenEducation', 2, NULL),
	(9, 'InfraBondsOrMFs', 2, NULL),
	(10, 'OtherInvestments', 2, NULL),
	(11, 'NationalPensionScheme', 3, '200000'),
	(12, 'InvestmentDemat', 4, '25000'),
	(13, 'HealthInsurance&Checkup', 5, '25000'),
	(14, 'HealthInsurance&CheckupParent', 5, '25000'),
	(15, 'Disability40-80%', 6, '75000'),
	(16, 'DisabilityGreaterThan80', 6, '125000'),
	(17, 'MedicalTreatment', 7, '40000'),
	(18, 'MedicalTreatmentSeniorCitizen', 7, '100000'),
	(19, 'DonationToCharity', 8, null),
	(20, 'SelfDisability40-80%', 9, '75000'),
	(21, 'SelfDisabilityGreaterThan80', 9, '125000');
SET IDENTITY_INSERT SubSections OFF;

SET IDENTITY_INSERT Years ON;

INSERT INTO Years(Id, Year) VALUES
	(1, 2019),
	(2, 2020),
	(3, 2021),
	(4, 2022),
	(5, 2023),
	(6, 2024);
SET IDENTITY_INSERT Years OFF;

SET IDENTITY_INSERT FinancialYear ON;

INSERT INTO FinancialYear(Id, FinancialYearStartId, FinancialYearEndId) VALUES
	(1, 1, 2),
	(2, 2, 3),
	(3, 3, 4),
	(4, 4, 5),
	(5, 5, 6);
SET IDENTITY_INSERT FinancialYear OFF;

INSERT INTO OldRegime(FinancialYearId, OldRegimeYearId) VALUES
	(1, null),
	(2, 1),
	(3, 1),
	(4, 1),
	(5, 1);

SET IDENTITY_INSERT Slab ON;

INSERT INTO Slab(Id, SlabNumber, Limit, PercentOfTax, FinantialYearId) VALUES
	(1, 1, 300000, 0, 5),
	(2, 2, 300000, 0.05, 5),
	(3, 3, 300000, 0.1, 5),
	(4, 4, 300000, 0.15, 5),
	(5, 5, 300000, 0.2, 5),
	(6, 6, 0, 0.3, 5),
	(7, 1, 250000, 0, 4),
	(8, 2, 250000, 0.05, 4),
	(9, 3, 250000, 0.1, 4),
	(10, 4, 250000, 0.15, 4),
	(11, 5, 250000, 0.2, 4),
	(12, 6, 250000, 0.25, 4),
	(13, 7, 0, 0.3, 4),
	(14, 1, 250000, 0, 3),
	(15, 2, 250000, 0.05, 3),
	(16, 3, 250000, 0.1, 3),
	(17, 4, 250000, 0.15, 3),
	(18, 5, 250000, 0.2, 3),
	(19, 6, 250000, 0.25, 3),
	(20, 7, 0, 0.3, 3),
	(21, 1, 250000, 0, 3),
	(22, 2, 250000, 0.05, 2),
	(23, 3, 250000, 0.1, 2),
	(24, 4, 250000, 0.15, 2),
	(25, 5, 250000, 0.2, 2),
	(26, 6, 250000, 0.25, 2),
	(27, 7, 0, 0.3, 2),
	(28, 1, 250000, 0, 1),
	(29, 2, 250000, 0.05, 1),
	(30, 3, 500000, 0.2, 1),
	(31, 4, 0, 0.3, 1);

SET IDENTITY_INSERT Slab OFF;
