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
