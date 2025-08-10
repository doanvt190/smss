-- Create Schedules table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Schedules' AND xtype='U')
BEGIN
    CREATE TABLE [Schedules] (
        [ScheduleID] int NOT NULL IDENTITY(1,1),
        [ClassID] int NOT NULL,
        [DayOfWeek] nvarchar(20) NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [Room] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Schedules] PRIMARY KEY ([ScheduleID])
    );
END

-- Add foreign key constraint if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Schedules_Classes')
BEGIN
    ALTER TABLE [Schedules] 
    ADD CONSTRAINT [FK_Schedules_Classes] 
    FOREIGN KEY ([ClassID]) REFERENCES [Classes] ([ClassID]);
END

-- Insert migration record
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20250805180636_CreateSchedulesTable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) 
    VALUES ('20250805180636_CreateSchedulesTable', '8.0.0');
END 