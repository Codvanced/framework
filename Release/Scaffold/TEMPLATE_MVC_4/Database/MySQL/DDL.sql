/* USE DATABASE */
USE mysql;

/* CRIAÇÃO DO DATABASE SE NÃO EXISTIR */
CREATE DATABASE IF NOT EXISTS IoC_FW;

/* USE DATABASE */
USE IoC_FW;

/* ---------- SE OS OBJETOS JÁ EXISTIREM - DROP ---------- */
DROP TABLE IF EXISTS Person;
DROP TABLE IF EXISTS Ocupation;
DROP TABLE IF EXISTS ArtistGenre;
DROP TABLE IF EXISTS Genre;
DROP TABLE IF EXISTS Artist;
DROP TABLE IF EXISTS WT_NOTICIA;

/* ---------- TABELAS TRANSACIONAIS ---------- */
CREATE TABLE Person
(
    IdPerson            INT				        NOT NULL PRIMARY KEY AUTO_INCREMENT
,   IdOcupation         INT				        NOT NULL
,   PersonName			    VARCHAR(200)	    NOT NULL
,   Gender              VARCHAR(50)		        NULL
,   Created             TIMESTAMP         NOT NULL DEFAULT CURRENT_TIMESTAMP()
,   Updated             TIMESTAMP			        NULL
,   Activated           BIT				        NOT NULL DEFAULT 1
);

CREATE TABLE Ocupation
(
    IdOcupation			    INT				        NOT NULL PRIMARY KEY AUTO_INCREMENT
,   OcupationName       VARCHAR(200)      NOT NULL
,   Created				      TIMESTAMP		      NOT NULL DEFAULT CURRENT_TIMESTAMP()
,   Updated				      TIMESTAMP		        	NULL
,   Activated			      BIT				        NOT NULL DEFAULT 1
);

CREATE TABLE Artist
(
    IdArtist			      INT				          NOT NULL PRIMARY KEY AUTO_INCREMENT
,   Name				        VARCHAR(200)        NOT NULL
);

CREATE TABLE Genre
(
    IdGenre				      INT				          NOT NULL PRIMARY KEY AUTO_INCREMENT
,   Name				        VARCHAR(200)        NOT NULL
);

CREATE TABLE ArtistGenre
(
    IdArtistGenre	      INT         				NOT NULL PRIMARY KEY AUTO_INCREMENT
,   IdArtist			      INT         				NOT NULL
,   IdGenre				      INT         				NOT NULL
);

/* Tabela para teste de CRUD */
CREATE TABLE WT_NOTICIA
(
    ID_NOTICIA 		      INT 			          NOT NULL PRIMARY KEY AUTO_INCREMENT
,   DSC_TITULO 			    VARCHAR(150)        NOT NULL
,   DSC_DESCRICAO 		  TEXT                NOT NULL
,   DSC_AUTOR			      VARCHAR(100)            NULL
,   DAT_NOTICIA 		    TIMESTAMP 		      NOT NULL
,   DAT_CADASTRO 		    TIMESTAMP 		      NOT NULL
,   DAT_ALTERACAO 		  TIMESTAMP 		        	NULL
);

/* ---------- CRIAÇÃO DE CHAVES Estrangeiras ---------- */
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
