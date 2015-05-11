/*-------------------------------------------------------------------------------------------------------------*/
/*------------------- Script para Criação da Base de Dados de Testes do Codvanced Framework (IOC FW) ----------*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Cria a base de dados se não existir */
IF(DB_ID('IoC_FW') IS NULL)
	CREATE DATABASE IoC_FW
	GO

/* Usa a base de dados criada */
USE IoC_FW
GO

/*------------------------- Apaga as Tabelas se Existirem -----------------------------------------------------*/
IF(OBJECT_ID('Person') IS NOT NULL)
	DROP TABLE Person
GO

IF(OBJECT_ID('Ocupation') IS NOT NULL)
	DROP TABLE Ocupation
GO

IF(OBJECT_ID('ArtistGenre') IS NOT NULL)
	DROP TABLE ArtistGenre
GO

IF(OBJECT_ID('Genre') IS NOT NULL)
	DROP TABLE Genre
GO
	
IF(OBJECT_ID('Artist') IS NOT NULL)
	DROP TABLE Artist
GO

IF(OBJECT_ID('News') IS NOT NULL)
	DROP TABLE News
GO

/*------------------------- Cria as Tabelas Transacionais -----------------------------------------------------*/
CREATE TABLE Person
(
	IdPerson			INT				NOT NULL IDENTITY(1,1)
,	IdOcupation			INT				NOT NULL
,	PersonName			VARCHAR(200)	NOT NULL
,	Gender				VARCHAR(50)			NULL
,	Created				DATETIME2		NOT NULL DEFAULT GETDATE()
,	Updated				DATETIME2			NULL
,	Activated			BIT				NOT NULL DEFAULT 1
)

CREATE TABLE Ocupation
(
	IdOcupation			INT				NOT NULL IDENTITY(1,1)
,	OcupationName		VARCHAR(200)	NOT NULL
,	Created				DATETIME2		NOT NULL DEFAULT GETDATE()
,	Updated				DATETIME2			NULL
,	Activated			BIT				NOT NULL DEFAULT 1
)

CREATE TABLE Artist
(
	IdArtist			INT				NOT NULL IDENTITY(1, 1)
,	Name				VARCHAR(200)	NOT NULL
)

CREATE TABLE Genre
(
	IdGenre				INT				NOT NULL IDENTITY(1, 1)
,	Name				VARCHAR(200)	NOT NULL
)

CREATE TABLE ArtistGenre
(
	IdArtistGenre		INT				NOT NULL IDENTITY(1, 1)
,	IdArtist			INT				NOT NULL
,	IdGenre				INT				NOT NULL
)

CREATE TABLE News
(
	IdNews 				INT 			NOT NULL IDENTITY(1,1)
,	Title 				VARCHAR(150) 	NOT NULL
,	NewsDescription		TEXT 			NOT NULL
,	Author				VARCHAR(100) 		NULL
,	NewsDate	 		DATETIME2 		NOT NULL DEFAULT GETDATE()
,	Created				DATETIME2 		NOT NULL DEFAULT GETDATE()
,	Updated	 			BIT		 			NULL
)

/*------------------------- Criação de Chaves Primárias -------------------------------------------------------*/
ALTER TABLE		Person
ADD CONSTRAINT	PK_Person
PRIMARY KEY		(IdPerson)

ALTER TABLE		Ocupation
ADD CONSTRAINT	PK_Ocupation
PRIMARY KEY		(IdOcupation)

ALTER TABLE		Artist
ADD CONSTRAINT	PK_Artist
PRIMARY KEY		(IdArtist)

ALTER TABLE		Genre
ADD CONSTRAINT	PK_Genre
PRIMARY KEY		(IdGenre)

ALTER TABLE		ArtistGenre
ADD CONSTRAINT	PK_ArtistGenre
PRIMARY KEY		(IdArtistGenre)

ALTER TABLE 	News
ADD CONSTRAINT 	PK_Nerws
PRIMARY KEY 	(IdNews)

/*------------------------- Criação de Chaves Extrangeiras ----------------------------------------------------*/
ALTER TABLE		Person
ADD CONSTRAINT	FK_Ocupation
FOREIGN KEY		(IdOcupation)
REFERENCES		Ocupation(IdOcupation)

ALTER TABLE		ArtistGenre
ADD CONSTRAINT	FK_ArtistGenre_Artist
FOREIGN KEY		(IdArtist)
REFERENCES		Artist (IdArtist)

ALTER TABLE		ArtistGenre
ADD CONSTRAINT	FK_ArtistGenre_Genre
FOREIGN KEY		(IdGenre)
REFERENCES		Genre (IdGenre)