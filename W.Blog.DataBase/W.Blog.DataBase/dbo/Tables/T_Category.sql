CREATE TABLE [dbo].[T_Category] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (200) NOT NULL,
    [ParentId]   NVARCHAR (100) NOT NULL,
    [Order]      INT            NOT NULL,
    [IsDelete]   INT            NOT NULL,
    [CreateTime] DATETIME  NOT NULL,
    CONSTRAINT [PK_T_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'类别名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Category', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'类别父级Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Category', @level2type = N'COLUMN', @level2name = N'ParentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序字段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_Category', @level2type = N'COLUMN', @level2name = N'Order';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'逻辑删除字段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Category',
    @level2type = N'COLUMN',
    @level2name = N'IsDelete'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Category',
    @level2type = N'COLUMN',
    @level2name = N'CreateTime'