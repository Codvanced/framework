/*-------------------------------------------------------------------------------------------------------------*/
/*------------- Script para Criação da Tabela de Logs para a Base de Dados do Codvanced Framework (IOC FW) ----*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Cria a base de dados se não existir */
CREATE DATABASE IF NOT EXISTS IoC_FW;

/* Usa a base de dados criada */
USE IoC_FW;

/*------------------------- Apaga as Tabela se Existir -------------------------------------------------------*/
DROP TABLE IF EXISTS Log;

/*------------------------- Criação da Tabela Transacional ---------------------------------------------------*/		
CREATE TABLE Log ( 
	LogId 					INT					NOT NULL
,	Date 					TIMESTAMP			NOT NULL  
,	Thread 					VARCHAR(255) 		NOT NULL 
,	Level 					VARCHAR(20) 		NOT NULL
,	Logger 					VARCHAR(255) 		NOT NULL
,	Message 				VARCHAR(4000) 		NOT NULL 
,	StackTrace 				VARCHAR(4000) 			NULL 
);

/*------------------------- Criação de Chave Primária --------------------------------------------------------*/
ALTER TABLE		Log
ADD CONSTRAINT 	PK_Log
PRIMARY KEY		(LogId);

ALTER TABLE     Log
CHANGE          LogId	LogId	INT 	NOT NULL 	AUTO_INCREMENT;