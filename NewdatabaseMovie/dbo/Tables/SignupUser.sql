CREATE TABLE [dbo].[SignupUser] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [Email]           NVARCHAR (MAX) NOT NULL,
    [Password]        NVARCHAR (MAX) NOT NULL,
    [ConfirmPassword] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_SignupUser] PRIMARY KEY CLUSTERED ([ID] ASC)
);

