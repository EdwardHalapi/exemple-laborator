CREATE TABLE [dbo].[OrderHeader] (
    [OrderId] INT          IDENTITY (1, 1) NOT NULL,
    [Address] VARCHAR (1)  NOT NULL,
    [Total]   DECIMAL (18) NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([OrderId] ASC)
);

