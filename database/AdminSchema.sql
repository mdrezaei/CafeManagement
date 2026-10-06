IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    IF SCHEMA_ID(N'admin') IS NULL EXEC(N'CREATE SCHEMA [admin];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE TABLE [admin].[Customers] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [PhoneNumber] nvarchar(25) NOT NULL,
        [Password] nvarchar(256) NULL,
        [SubmitDate] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE TABLE [admin].[Employees] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Password] nvarchar(256) NOT NULL,
        [PhoneNumber] nvarchar(25) NOT NULL,
        [JoinedDate] datetime2 NOT NULL,
        [Section] int NOT NULL,
        [Level] int NOT NULL,
        [SecondarySection] int NULL,
        [SecondaryLevel] int NULL,
        [IsDeleted] bit NOT NULL,
        [Description] nvarchar(500) NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE TABLE [admin].[MenuItems] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [Price] decimal(18,0) NOT NULL,
        [IsAvailable] bit NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [ThumbNailUrl] nvarchar(max) NULL,
        [PicturesUrl] nvarchar(max) NULL,
        [Category] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_MenuItems] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE TABLE [admin].[Orders] (
        [Id] uniqueidentifier NOT NULL,
        [Status] int NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [CustomerNote] nvarchar(500) NULL,
        [TableId] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [SourceOrderId] uniqueidentifier NOT NULL,
        [AssignedWaiterId] uniqueidentifier NULL,
        [AssignedBaristaId] uniqueidentifier NULL,
        [AssignedChefId] uniqueidentifier NULL,
        [AssignedCashierId] uniqueidentifier NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE TABLE [admin].[OutboxMessages] (
        [Id] uniqueidentifier NOT NULL,
        [EventType] nvarchar(max) NOT NULL,
        [EventData] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ProcessedAt] datetime2 NULL,
        [IsProcessed] bit NOT NULL,
        [RetryCount] int NOT NULL,
        CONSTRAINT [PK_OutboxMessages] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE TABLE [admin].[Tables] (
        [Id] int NOT NULL IDENTITY,
        [TableNumber] nvarchar(100) NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Tables] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE TABLE [admin].[MOrderItem] (
        [Id] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [MenuItemId] uniqueidentifier NOT NULL,
        [OrderedItemName] nvarchar(100) NOT NULL,
        [OrderedUnitPrice] decimal(18,0) NOT NULL,
        [MOrderId] uniqueidentifier NULL,
        CONSTRAINT [PK_MOrderItem] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MOrderItem_Orders_MOrderId] FOREIGN KEY ([MOrderId]) REFERENCES [admin].[Orders] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MOrderItem_MOrderId] ON [admin].[MOrderItem] ([MOrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_AssignedBaristaId] ON [admin].[Orders] ([AssignedBaristaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_AssignedCashierId] ON [admin].[Orders] ([AssignedCashierId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_AssignedChefId] ON [admin].[Orders] ([AssignedChefId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_AssignedWaiterId] ON [admin].[Orders] ([AssignedWaiterId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000749_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260822000749_InitialCreate', N'10.0.10');
END;

COMMIT;
GO

