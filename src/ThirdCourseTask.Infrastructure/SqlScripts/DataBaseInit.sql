IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tasks')
BEGIN
    CREATE TABLE dbo.Tasks (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Title       NVARCHAR(200)      NOT NULL,
        Description NVARCHAR(MAX)      NULL,
        IsCompleted BIT                NOT NULL DEFAULT(0),
        CreatedAt   DATETIME2(0)       NOT NULL DEFAULT (SYSUTCDATETIME())
    );
END
