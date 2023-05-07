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