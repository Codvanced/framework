/*-------------------------------------------------------------------------------------------------------------*/
/*------------- Script para Criação da Tabela de Logs para a Base de Dados do Codvanced Framework (IOC FW) ----*/
/*-------------------------------------------------------------------------------------------------------------*/

/*------------------------- Apaga as Tabela se Existir -------------------------------------------------------*/
DROP TABLE IF EXISTS Log;

/*------------------------- Criação da Tabela Transacional ---------------------------------------------------*/		
CREATE TABLE Log ( 
	LogId 					INTEGER				NOT NULL PRIMARY KEY
,	Date 					TIMESTAMP			NOT NULL  
,	Thread 					VARCHAR(255) 		NOT NULL 
,	Level 					VARCHAR(20) 		NOT NULL
,	Logger 					VARCHAR(255) 		NOT NULL
,	Message 				VARCHAR(4000) 		NOT NULL 
,	StackTrace 				VARCHAR(4000) 			NULL 
);