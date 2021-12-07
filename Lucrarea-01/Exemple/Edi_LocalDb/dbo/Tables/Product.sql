CREATE TABLE [dbo].[Product] (
    [ProductId] INT          IDENTITY (1, 1) NOT NULL,
    [Code]      VARCHAR (7)  NOT NULL,
    [Stoc]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);

