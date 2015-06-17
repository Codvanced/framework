/*-------------------------------------------------------------------------------------------------------------*/
/*------------------- Script para Criação da Base de Dados de Testes do Codvanced Framework (IOC FW) ----------*/
/*-------------------------------------------------------------------------------------------------------------*/

/*------------------------- Apaga as Tabelas se Existirem -----------------------------------------------------*/
DROP TABLE IF EXISTS Person;
DROP TABLE IF EXISTS Ocupation;
DROP TABLE IF EXISTS ArtistGenre;
DROP TABLE IF EXISTS Genre;
DROP TABLE IF EXISTS Artist;
DROP TABLE IF EXISTS News;

/*------------------------- Cria as Tabelas Transacionais -----------------------------------------------------*/
CREATE TABLE Person
(
	IdPerson		INTEGER         			NOT NULL PRIMARY KEY
,	IdOcupation		INTEGER         			NOT NULL
,	PersonName		VARCHAR(200)    			NOT NULL
,	Gender			VARCHAR(50)         			NULL
,	Created			DATETIME					NOT NULL DEFAULT (DATETIME('NOW', 'LOCALTIME'))
,	Updated			DATETIME           				NULL
,	Activated		BIT							NOT NULL DEFAULT 1
,   FOREIGN KEY (IdOcupation) 
	REFERENCES 	Ocupation (IdOcupation)
);

CREATE TABLE Ocupation
(
	IdOcupation		INTEGER						NOT NULL PRIMARY KEY
,	OcupationName	VARCHAR(200)				NOT NULL
,	Created			DATETIME					NOT NULL DEFAULT (DATETIME('NOW', 'LOCALTIME'))
,	Updated			DATETIME						NULL
,	Activated		BIT							NOT NULL DEFAULT 1
);

CREATE TABLE Artist
(
	IdArtist		INTEGER						NOT NULL PRIMARY KEY
,	Name			VARCHAR(200)    			NOT NULL
);

CREATE TABLE Genre
(
	IdGenre			INTEGER						NOT NULL PRIMARY KEY
,	Name			VARCHAR(200)    			NOT NULL
);

CREATE TABLE ArtistGenre
(
	IdArtistGenre	INTEGER						NOT NULL PRIMARY KEY
,	IdArtist		INTEGER						NOT NULL
,	IdGenre			INTEGER						NOT NULL
,   FOREIGN KEY (IdArtist) 
	REFERENCES 	Artist (IdArtist)
,   FOREIGN KEY (IdGenre) 
	REFERENCES 	Genre (IdGenre)
);

CREATE TABLE News
(
	IdNews 				INT 					NOT NULL PRIMARY KEY
,	Title 				VARCHAR(150) 			NOT NULL
,	NewsDescription		TEXT 					NOT NULL
,	Author				VARCHAR(100) 				NULL
,	NewsDate	 		TIMESTAMP 				NOT NULL
,	Created				TIMESTAMP 				NOT NULL
,	Updated	 			TIMESTAMP 					NULL
);