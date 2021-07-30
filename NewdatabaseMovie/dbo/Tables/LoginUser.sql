CREATE TABLE [dbo].[LoginUser] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Email]      NVARCHAR (MAX) NOT NULL,
    [Password]   NVARCHAR (MAX) NOT NULL,
    [RememberMe] BIT            NOT NULL,
    CONSTRAINT [PK_LoginUser] PRIMARY KEY CLUSTERED ([ID] ASC)
);

