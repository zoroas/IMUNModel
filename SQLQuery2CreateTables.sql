CREATE TABLE [dbo].[Cards] (
    [CardId]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [FirstName]   VARCHAR (200) NOT NULL,
    [LastName]    VARCHAR (200) NOT NULL,
    [Country]     VARCHAR (200) DEFAULT ('') NOT NULL,
    [PicturePath] VARCHAR (200) DEFAULT ('') NOT NULL,
    [CreatedDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    [PositionId]  BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([CardId] ASC)
);
