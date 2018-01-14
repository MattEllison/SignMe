CREATE TABLE [dbo].[ActivityHistory] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [UserID]     INT      NOT NULL,
    [InsertDate] DATETIME DEFAULT (getutcdate()) NOT NULL,
    [StatusID]   INT      NOT NULL,
    [DocumentID] INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [PK_DocumentID] FOREIGN KEY ([DocumentID]) REFERENCES [dbo].[Document] ([Id]),
    CONSTRAINT [PK_Status] FOREIGN KEY ([StatusID]) REFERENCES [dbo].[DocumentStatus] ([Id])
);

