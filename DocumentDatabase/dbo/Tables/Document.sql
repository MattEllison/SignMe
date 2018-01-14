CREATE TABLE [dbo].[Document] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [DocumentName]  NVARCHAR (MAX) NOT NULL,
    [Base64]        VARCHAR (MAX)  NOT NULL,
    [SignedBased64] VARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

