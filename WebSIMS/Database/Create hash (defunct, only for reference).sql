Use SIMS
Select Users
CREATE LOGIN admin_user WITH PASSWORD = 'AdminPassword123';
-- Maps the 'admin_user' login to a user named 'admin_user' in the current database.
CREATE USER admin_user FOR LOGIN admin_user;
-- Optional: Grant this user powerful permissions, like 'db_owner'.
-- This gives the user full control over the database schema and data.
ALTER ROLE db_owner ADD MEMBER admin_user;
PRINT 'Admin login and user created successfully.';
GO

CREATE LOGIN faculty_user WITH PASSWORD = 'FacultyPassword123';
-- Maps the 'faculty_user' login to a user named 'faculty_user' in the current database.
CREATE USER faculty_user FOR LOGIN faculty_user;

-- Optional: Grant this user permissions to read and write data.
-- 'db_datareader' allows reading from any table.
-- 'db_datawriter' allows writing to any table.
ALTER ROLE db_datareader ADD MEMBER faculty_user;
ALTER ROLE db_datawriter ADD MEMBER faculty_user;
PRINT 'Faculty login and user created successfully.';
GO

CREATE LOGIN student_user WITH PASSWORD = 'StudentPassword123';
-- Maps the 'student_user' login to a user named 'student_user' in the current database.
CREATE USER student_user FOR LOGIN student_user;
-- Optional: Grant this user only read access.
ALTER ROLE db_datareader ADD MEMBER student_user;
-- If students need to write data (e.g., submit assignments), also add:
-- ALTER ROLE db_datawriter ADD MEMBER student_user;
PRINT 'Student login and user created successfully.';
GO