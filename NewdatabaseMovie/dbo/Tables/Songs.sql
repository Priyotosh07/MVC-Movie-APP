CREATE TABLE [dbo].[Songs] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Songname]         NVARCHAR (MAX) NOT NULL,
    [SongAuthor]       NVARCHAR (MAX) NOT NULL,
    [SongGenre]        NVARCHAR (MAX) NOT NULL,
    [SongRelaeaseDate] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Songs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

