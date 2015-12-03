/*-------------------------------------------------------------------------------------------------------------*/
/*-------------------------- Script para Inserção da Base de Dados do Codvanced Framework (IOC FW) ------------*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Válida se a base de dados existe para fazer a carga */
IF(DB_ID('IoC_FW') IS NOT NULL)
	/* Usa a base de dados criada */
	USE IoC_FW;
	GO
	
	/*--- Faz a carga se a tabela Ocupation existir ---*/
	IF(OBJECT_ID('Ocupation') IS NOT NULL)
		INSERT INTO Ocupation (OcupationName)
		VALUES 
			('Ocupation 1')
		,	('Ocupation 2')
		,	('Ocupation 3')
	GO
	
	/*--- Faz a carga se a tabela Person existir ---*/
	IF(OBJECT_ID('Person') IS NOT NULL)
		INSERT INTO Person (PersonName, IdOcupation, Gender)
		VALUES
			('Person 1', 1, 'Masculino')
		,	('Person 2', 2, 'Feminino')
		,	('Person 3', 3, 'Masculino')
	GO

	/*--- Faz a carga se a tabela Artist existir ---*/
	IF(OBJECT_ID('Artist') IS NOT NULL)
		INSERT INTO Artist (Name) 
		VALUES
			('Artista1')
		,	('Artista2')
		,	('Artista3')
		,	('Artista4')
		,	('Artista5')
	GO

	/*--- Faz a carga se a tabela Genre existir ---*/
	IF(OBJECT_ID('Genre') IS NOT NULL)
		INSERT INTO Genre (Name) 
		VALUES
			('Genre1')
		,	('Genre2')
		,	('Genre3')
		,	('Genre4')
	GO

	/*--- Faz a carga se a tabela ArtistGenre e as tabelas Artist e Genre existirem ---*/
	IF(OBJECT_ID('ArtistGenre') IS NOT NULL AND OBJECT_ID('Artist') IS NOT NULL AND OBJECT_ID('Genre') IS NOT NULL)
		INSERT INTO ArtistGenre(IdArtist, IdGenre)
		VALUES
			(1, 1)
		,	(1, 2)
		,	(1, 3)
		,	(2, 1)
		,	(2, 2)
		,	(3, 1)
		,	(3, 2)
		,	(3, 3)
		,	(3, 4)
		,	(4, 1)
	GO
GO