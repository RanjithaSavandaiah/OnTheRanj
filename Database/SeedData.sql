-- SQL Server seed script for OnTheRanj
-- This script clears tables and inserts fresh data for Users, ProjectCodes, ProjectAssignments, Timesheets

-- Clear existing data (order matters due to FKs)
DELETE FROM Timesheets;
DELETE FROM ProjectAssignments;
DELETE FROM ProjectCodes;
DELETE FROM Users;
GO

-- Seed Users (Manager and Employees)
-- All users use password: Manager@123 (bcrypt hash below)
-- Manager password: Manager@123 (bcrypt hash below)
-- Employee password: Employee@123 (bcrypt hash below)
-- Manager@123: $2a$11$XM0HpQJ.WqGzLvJNqSv.Ke2QxPqP5kYE8x1FKV0PkMKOyQD8QLqHW
-- Employee@123: $2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu
INSERT INTO Users (Id, FullName, Email, PasswordHash, Role, IsActive, CreatedAt) VALUES
(1, 'John Manager', 'manager@ontheranj.com', '$2a$11$XM0HpQJ.WqGzLvJNqSv.Ke2QxPqP5kYE8x1FKV0PkMKOyQD8QLqHW', 'Manager', 1, GETDATE()),
(2, 'Alice Employee', 'alice@ontheranj.com', '$2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu', 'Employee', 1, GETDATE()),
(3, 'Bob Employee', 'bob@ontheranj.com', '$2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu', 'Employee', 1, GETDATE()),
(4, 'Charlie Employee', 'charlie@ontheranj.com', '$2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu', 'Employee', 1, GETDATE());
GO

-- Seed Project Codes
INSERT INTO ProjectCodes (Id, Code, ProjectName, ClientName, IsBillable, Status, CreatedAt, CreatedBy) VALUES
(1, 'PROJ001', 'E-Commerce Website', 'TechCorp Ltd', 1, 'Active', GETDATE(), 1),
(2, 'PROJ002', 'Mobile App Development', 'InnovateSoft', 1, 'Active', GETDATE(), 1),
(3, 'PROJ003', 'Data Analytics Platform', 'DataViz Inc', 1, 'Active', GETDATE(), 1),
(4, 'INTERNAL001', 'Internal Training', 'OnTheRanj', 0, 'Active', GETDATE(), 1),
(5, 'INTERNAL002', 'Team Building Activities', 'OnTheRanj', 0, 'Active', GETDATE(), 1),
(6, 'PROJ004', 'Legacy System Migration', 'OldTech Corp', 1, 'Inactive', GETDATE(), 1);
GO

-- Seed Project Assignments
INSERT INTO ProjectAssignments (Id, EmployeeId, ProjectCodeId, StartDate, EndDate, CreatedAt, CreatedBy) VALUES
(1, 2, 1, '2025-11-25', '2026-02-25', GETDATE(), 1),
(2, 2, 4, '2025-11-25', NULL, GETDATE(), 1),
(3, 3, 2, '2025-11-25', NULL, GETDATE(), 1),
(4, 4, 3, '2025-11-25', NULL, GETDATE(), 1);
GO

-- Seed Timesheets (minimal, adjust as needed)
INSERT INTO Timesheets (Id, EmployeeId, ProjectCodeId, Date, HoursWorked, Description, Status, CreatedAt) VALUES
(1, 2, 1, '2025-11-20', 8.00, 'Implemented user authentication module', 'Approved', GETDATE()),
(2, 2, 1, '2025-11-21', 7.50, 'Developed product catalog API endpoints', 'Approved', GETDATE()),
(3, 2, 4, '2025-11-21', 0.50, 'Attended Angular training session', 'Approved', GETDATE());
GO
