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
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    IF SCHEMA_ID(N'menu') IS NULL EXEC(N'CREATE SCHEMA [menu];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    CREATE TABLE [menu].[Customers] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Password] nvarchar(256) NULL,
        [PhoneNumber] nvarchar(25) NOT NULL,
        [SubmitDate] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    CREATE TABLE [menu].[MenuItems] (
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
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    CREATE TABLE [menu].[Orders] (
        [Id] uniqueidentifier NOT NULL,
        [Status] int NOT NULL,
        [CustomerNote] nvarchar(500) NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [TableId] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    CREATE TABLE [menu].[OutboxMessages] (
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
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    CREATE TABLE [menu].[Tables] (
        [Id] int NOT NULL IDENTITY,
        [TableNumber] nvarchar(100) NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Tables] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    CREATE TABLE [menu].[OrderItem] (
        [Id] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [MenuItemId] uniqueidentifier NOT NULL,
        [OrderedItemName] nvarchar(100) NOT NULL,
        [OrderedUnitPrice] decimal(18,0) NOT NULL,
        [OrderId] uniqueidentifier NULL,
        CONSTRAINT [PK_OrderItem] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItem_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [menu].[Orders] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderItem_OrderId] ON [menu].[OrderItem] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260822000857_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260822000857_InitialCreate', N'10.0.10');
END;

COMMIT;
GO

