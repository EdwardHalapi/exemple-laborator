CREATE TABLE [dbo].[OrderLine] (
    [OrderLineId] INT          IDENTITY (1, 1) NOT NULL,
    [Quantity]    DECIMAL (18) NOT NULL,
    [Price]       DECIMAL (18) NULL,
    CONSTRAINT [PK_OrderLine] PRIMARY KEY CLUSTERED ([OrderLineId] ASC)
);

