CREATE TABLE [dbo].[T_Article]
(
	  [Id]         INT            IDENTITY (1, 1) NOT NULL, 
    [Title] NVARCHAR(200) NOT NULL, 
    [Content] NVARCHAR(MAX) NOT NULL, 
    [CategoryId] INT NOT NULL, 
    [IsImportance] BIT NOT NULL, 
    [IsEnd] BIT NOT NULL,
    [IsDelete]   INT            NOT NULL,
    [CreateTime] DATETIME  NOT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'标题',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Article',
    @level2type = N'COLUMN',
    @level2name = N'Title'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'内容',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Article',
    @level2type = N'COLUMN',
    @level2name = N'Content'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'类别Id',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Article',
    @level2type = N'COLUMN',
    @level2name = N'CategoryId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否置顶',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Article',
    @level2type = N'COLUMN',
    @level2name = N'IsImportance'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否完结',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Article',
    @level2type = N'COLUMN',
    @level2name = N'IsEnd'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'逻辑删除',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Article',
    @level2type = N'COLUMN',
    @level2name = N'IsDelete'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'创建时间',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Article',
    @level2type = N'COLUMN',
    @level2name = N'CreateTime'