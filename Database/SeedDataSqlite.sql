-- SQLite-compatible seed data for OnTheRanj

-- Users
-- All users use password: Manager@123 (bcrypt hash below)
-- Manager password: Manager@123 (bcrypt hash below)
-- Employee password: Employee@123 (bcrypt hash below)
-- Manager@123: $2a$11$XM0HpQJ.WqGzLvJNqSv.Ke2QxPqP5kYE8x1FKV0PkMKOyQD8QLqHW
-- Employee@123: $2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu
INSERT INTO Users (Id, FullName, Email, PasswordHash, Role, IsActive, CreatedAt)
VALUES 
(1, 'John Manager', 'manager@ontheranj.com', '$2a$11$XM0HpQJ.WqGzLvJNqSv.Ke2QxPqP5kYE8x1FKV0PkMKOyQD8QLqHW', 'Manager', 1, CURRENT_TIMESTAMP),
(2, 'Alice Employee', 'alice@ontheranj.com', '$2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu', 'Employee', 1, CURRENT_TIMESTAMP),
(3, 'Bob Employee', 'bob@ontheranj.com', '$2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu', 'Employee', 1, CURRENT_TIMESTAMP),
(4, 'Charlie Employee', 'charlie@ontheranj.com', '$2a$11$ukmPBZ2904y6CO.TXbTHJOIVSSuEBGGHIOyt.mtUXRPfKU66W3Mdu', 'Employee', 1, CURRENT_TIMESTAMP);

-- Project Codes
INSERT INTO ProjectCodes (Id, Code, ProjectName, ClientName, IsBillable, Status, CreatedAt, CreatedBy)
VALUES
(1, 'PROJ001', 'E-Commerce Website', 'TechCorp Ltd', 1, 'Active', CURRENT_TIMESTAMP, 1),
(2, 'PROJ002', 'Mobile App Development', 'InnovateSoft', 1, 'Active', CURRENT_TIMESTAMP, 1),
(3, 'PROJ003', 'Data Analytics Platform', 'DataViz Inc', 1, 'Active', CURRENT_TIMESTAMP, 1),
(4, 'INTERNAL001', 'Internal Training', 'OnTheRanj', 0, 'Active', CURRENT_TIMESTAMP, 1),
(5, 'INTERNAL002', 'Team Building Activities', 'OnTheRanj', 0, 'Active', CURRENT_TIMESTAMP, 1),
(6, 'PROJ004', 'Legacy System Migration', 'OldTech Corp', 1, 'Inactive', CURRENT_TIMESTAMP, 1);

-- Project Assignments (Alice: EmployeeId=2, ProjectCodeId=1, valid for 2025-11-23 to 2026-02-23)
INSERT INTO ProjectAssignments (Id, EmployeeId, ProjectCodeId, StartDate, EndDate, CreatedAt, CreatedBy)
VALUES
(1, 2, 1, '2025-11-23', '2026-02-23', CURRENT_TIMESTAMP, 1),
(2, 2, 4, '2025-11-23', NULL, CURRENT_TIMESTAMP, 1),
(3, 3, 2, '2025-10-23', '2026-03-23', CURRENT_TIMESTAMP, 1),
(4, 3, 4, '2025-09-23', NULL, CURRENT_TIMESTAMP, 1),
(5, 4, 3, '2025-09-23', '2026-04-23', CURRENT_TIMESTAMP, 1),
(6, 4, 5, '2025-11-23', '2026-01-23', CURRENT_TIMESTAMP, 1);

-- Sample Timesheets (for Alice)
INSERT INTO Timesheets (Id, EmployeeId, ProjectCodeId, Date, HoursWorked, Description, Status, CreatedAt)
VALUES
(1, 2, 1, '2025-12-15', 8, 'Worked on frontend features', 'Approved', CURRENT_TIMESTAMP),
(2, 2, 1, '2025-12-16', 6, 'Internal training session', 'Submitted', CURRENT_TIMESTAMP),
(3, 2, 1, '2025-12-17', 8, 'UI improvements', 'Submitted', CURRENT_TIMESTAMP);
