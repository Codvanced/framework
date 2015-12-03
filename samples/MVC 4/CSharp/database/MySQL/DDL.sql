/*-------------------------------------------------------------------------------------------------------------*/
/*------------------- Script para Criação da Base de Dados de Testes do Codvanced Framework (IOC FW) ----------*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Cria a base de dados se não existir */
CREATE DATABASE IF NOT EXISTS IoC_FW;

/* Usa a base de dados criada */
USE IoC_FW;

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
    IdPerson            INT				        NOT NULL
,   IdOcupation         INT				        NOT NULL
,   PersonName			VARCHAR(200)	    	NOT NULL
,   Gender              VARCHAR(50)		        	NULL
,   Created             TIMESTAMP         		NOT NULL	DEFAULT CURRENT_TIMESTAMP()
,   Updated             TIMESTAMP			        NULL
,   Activated           BIT				        NOT NULL 	DEFAULT 1
);

CREATE TABLE Ocupation
(
    IdOcupation			INT					    NOT NULL
,   OcupationName       VARCHAR(200)      		NOT NULL
,   Created				TIMESTAMP		      	NOT NULL 	DEFAULT CURRENT_TIMESTAMP()
,   Updated				TIMESTAMP		        	NULL
,   Activated			BIT				        NOT NULL 	DEFAULT 1
);

CREATE TABLE Artist
(
    IdArtist 	 		INT 					NOT NULL
,   Name				VARCHAR(200)        	NOT NULL
);

CREATE TABLE Genre
(
    IdGenre 			INT 					NOT NULL
,   Name 				VARCHAR(200)        	NOT NULL
);

CREATE TABLE ArtistGenre
(
    IdArtistGenre		INT 					NOT NULL
,   IdArtist 			INT         			NOT NULL
,   IdGenre 			INT         			NOT NULL
);

CREATE TABLE News
(
	IdNews 				INT 					NOT NULL
,	Title 				VARCHAR(150) 			NOT NULL
,	NewsDescription		TEXT 					NOT NULL
,	Author				VARCHAR(100) 				NULL
,	NewsDate	 		TIMESTAMP 				NOT NULL
,	Created				TIMESTAMP 				NOT NULL
,	Updated	 			TIMESTAMP 					NULL
);

/*------------------------- Criação de Chaves Primárias -------------------------------------------------------*/
ALTER TABLE		Person
ADD CONSTRAINT	PK_Person
PRIMARY KEY		(IdPerson);

ALTER TABLE     Person
CHANGE          IdPerson		IdPerson		INT 	NOT NULL 	AUTO_INCREMENT;

ALTER TABLE		Ocupation
ADD CONSTRAINT	PK_Ocupation
PRIMARY KEY		(IdOcupation);

ALTER TABLE     Ocupation
CHANGE          IdOcupation			IdOcupation 	INT 	NOT NULL 	AUTO_INCREMENT;

ALTER TABLE		Artist
ADD CONSTRAINT	PK_Artist
PRIMARY KEY		(IdArtist);

ALTER TABLE     Artist
CHANGE          IdArtist 			IdArtist 		INT 	NOT NULL 	AUTO_INCREMENT;

ALTER TABLE		Genre
ADD CONSTRAINT	PK_Genre
PRIMARY KEY		(IdGenre);

ALTER TABLE     Genre
CHANGE          IdGenre          	IdGenre 		INT 	NOT NULL 	AUTO_INCREMENT;

ALTER TABLE		ArtistGenre
ADD CONSTRAINT	PK_ArtistGenre
PRIMARY KEY		(IdArtistGenre);

ALTER TABLE     ArtistGenre
CHANGE          IdArtistGenre 		IdArtistGenre 	INT 	NOT NULL 	AUTO_INCREMENT;

ALTER TABLE 	News
ADD CONSTRAINT 	PK_Nerws
PRIMARY KEY 	(IdNews);

ALTER TABLE     News
CHANGE          IdNews 				IdNews 			INT 	NOT NULL 	AUTO_INCREMENT;

/*------------------------- Criação de Chaves Extrangeiras ----------------------------------------------------*/
ALTER TABLE     Person
ADD CONSTRAINT	FK_Ocupation
FOREIGN KEY     (IdOcupation)
REFERENCES      Ocupation(IdOcupation);

ALTER TABLE     ArtistGenre
ADD CONSTRAINT	FK_ArtistGenre_Artist
FOREIGN KEY     (IdArtist)
REFERENCES      Artist (IdArtist);

ALTER TABLE     ArtistGenre
ADD CONSTRAINT	FK_ArtistGenre_Genre
FOREIGN KEY     (IdGenre)
REFERENCES      Genre (IdGenre);
