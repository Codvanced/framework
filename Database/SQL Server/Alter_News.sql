/*-------------------------------------------------------------------------------------------------------------*/
/*------------------------- Script para Alteração da Base de Dados do Codvanced Framework (IOC FW) ------------*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Válida se a base de dados e a tabela existe para alterar a tabela */
IF(DB_ID('IoC_FW') IS NOT NULL AND OBJECT_ID('News') IS NOT NULL)
	/* Usa a base de dados criada */
	USE IoC_FW;
	GO
	
	IF((SELECT TOP 1 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'News' AND COLUMN_NAME = 'Priority') IS NULL)
		ALTER TABLE News
		ADD [Priority]	BIGINT	NOT NULL
		GO
GO


