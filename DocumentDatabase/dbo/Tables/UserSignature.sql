CREATE TABLE [dbo].[UserSignature] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [UserName]        NVARCHAR (255) NOT NULL,
    [SignatureBase64] VARCHAR (MAX)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

