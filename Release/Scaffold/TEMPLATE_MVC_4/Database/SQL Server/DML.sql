/* USE DATABASE */
USE IoC_FW
GO

/* --- INSERE OS REGISTROS SE OS OBJETOS JÁ EXISTIREM --- */
IF(OBJECT_ID('Ocupation') IS NOT NULL)
	INSERT INTO Ocupation (OcupationName)
	VALUES 
		('Ocupation 1')
	,	('Ocupation 2')
	,	('Ocupation 3')
GO

IF(OBJECT_ID('Person') IS NOT NULL)
	INSERT INTO Person (PersonName, IdOcupation, Gender)
	VALUES
		('Person 1', 1, 'Masculino')
	,	('Person 2', 2, 'Feminino')
	,	('Person 3', 3, 'Masculino')
GO

IF(OBJECT_ID('Artist') IS NOT NULL)
	INSERT INTO Artist (Name) 
	VALUES
		('Artista1')
	,	('Artista2')
	,	('Artista3')
	,	('Artista4')
	,	('Artista5')
GO

IF(OBJECT_ID('Genre') IS NOT NULL)
	INSERT INTO Genre (Name) 
	VALUES
		('Genre1')
	,	('Genre2')
	,	('Genre3')
	,	('Genre4')
GO

IF(OBJECT_ID('ArtistGenre') IS NOT NULL)
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

IF(OBJECT_ID('WT_NOTICIA') IS NOT NULL)
	INSERT INTO WT_NOTICIA
		(DSC_TITULO,
		DSC_DESCRICAO,
		DSC_AUTOR,
		DAT_NOTICIA,
		DAT_CADASTRO,
		DAT_ALTERACAO)
	VALUES
		('Titulo',
		'Descricao',
		'Autor',
		GETDATE(),
		GETDATE(),
		GETDATE()
		)
GO



SELECT 
	A.*
,	G.* 
FROM 
	ArtistGenre AG
INNER JOIN 
	Artist A
	ON AG.IdArtist = A.IdArtist
INNER JOIN 
	Genre G
	ON AG.IdGenre = G.IdGenre
	
SELECT * FROM Artist
SELECT * FROM Genre
SELECT * FROM ArtistGenre