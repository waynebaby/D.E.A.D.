CREATE TABLE [dbo].[Groups] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_dbo.Groups] PRIMARY KEY CLUSTERED ([Id] ASC)
);

