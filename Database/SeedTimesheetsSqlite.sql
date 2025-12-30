-- SQLite-compatible seed data for Timesheets
-- Insert a few sample timesheets for Alice and Bob

INSERT INTO Timesheets (EmployeeId, ProjectCodeId, Date, HoursWorked, Description, Status, CreatedAt)
VALUES
  (2, 1, '2025-12-15', 8, 'Worked on frontend features', 'Submitted', '2025-12-15'),
  (2, 4, '2025-12-16', 6, 'Internal training session', 'Submitted', '2025-12-16'),
  (3, 2, '2025-12-15', 7, 'API integration', 'Submitted', '2025-12-15'),
  (3, 2, '2025-12-16', 8, 'Bug fixes and testing', 'Draft', '2025-12-16'),
  (2, 1, '2025-12-17', 8, 'UI improvements', 'Draft', '2025-12-17');
