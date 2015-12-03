/*-------------------------------------------------------------------------------------------------------------*/
/*------------- Script para Cria��o da Tabela de Logs para a Base de Dados do Codvanced Framework (IOC FW) ----*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Cria a base de dados se n�o existir */
IF(DB_ID('IoC_FW') IS NULL)
	CREATE DATABASE IoC_FW;
	GO

/* Usa a base de dados criada */
USE IoC_FW;
GO

/*------------------------- Apaga as Tabela se Existir -------------------------------------------------------*/
IF(OBJECT_ID('Log') IS NOT NULL)
	DROP TABLE Log

/*------------------------- Cria��o da Tabela Transacional ---------------------------------------------------*/		
CREATE TABLE Log ( 
	LogId 					INT					NOT NULL		IDENTITY(1, 1)
,	[Date] 					DATETIME 			NOT NULL  
,	[Thread] 				NVARCHAR(255) 		NOT NULL 
,	[Level] 				NVARCHAR(20) 		NOT NULL
,	[Logger] 				NVARCHAR(255) 		NOT NULL
,	[Message] 				NVARCHAR(4000) 		NOT NULL 
,	[StackTrace] 			NVARCHAR(4000) 			NULL 
)

/*------------------------- Cria��o de Chave Prim�ria --------------------------------------------------------*/
ALTER TABLE 	Log
ADD CONSTRAINT 	PK_Log
PRIMARY KEY		(LogId)