/*-------------------------------------------------------------------------------------------------------------*/
/*------------------------- Script para Alteração da Base de Dados do Codvanced Framework (IOC FW) ------------*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Cria a base de dados se não existir */
CREATE DATABASE IF NOT EXISTS IoC_FW;

/* Usa a base de dados criada */
USE IoC_FW;
	
ALTER TABLE News
ADD	Priority	BIGINT	NOT NULL