CREATE TABLE [dbo].[DocumentStatus] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    [Code] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

