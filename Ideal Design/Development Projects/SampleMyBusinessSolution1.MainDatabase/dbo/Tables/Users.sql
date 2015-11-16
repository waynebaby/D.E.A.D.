CREATE TABLE [dbo].[Users] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
    [GroupId] INT           NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Users_dbo.Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_GroupId]
    ON [dbo].[Users]([GroupId] ASC);

