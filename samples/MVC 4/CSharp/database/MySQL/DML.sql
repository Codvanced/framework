/*-------------------------------------------------------------------------------------------------------------*/
/*-------------------------- Script para Inserção da Base de Dados do Codvanced Framework (IOC FW) ------------*/
/*-------------------------------------------------------------------------------------------------------------*/

/* Usa a base de dados criada */
USE IoC_FW;

/*--- Faz a carga se a tabela Ocupation ---*/
INSERT INTO Ocupation (OcupationName)
VALUES 
    ('Ocupation 1')
,   ('Ocupation 2')
,   ('Ocupation 3');

/*--- Faz a carga se a tabela Person ---*/
INSERT INTO Person (PersonName, IdOcupation, Gender)
VALUES
    ('Person 1', 1, 'Masculino')
,   ('Person 2', 2, 'Feminino')
,   ('Person 3', 3, 'Masculino');

/*--- Faz a carga se a tabela Artist ---*/
INSERT INTO Artist (Name) 
VALUES
    ('Artista1')
,   ('Artista2')
,   ('Artista3')
,   ('Artista4')
,   ('Artista5');

/*--- Faz a carga se a tabela Genre ---*/
INSERT INTO Genre (Name) 
VALUES
    ('Genre1')
,   ('Genre2')
,   ('Genre3')
,   ('Genre4');

/*--- Faz a carga se a tabela ArtistGenre ---*/
INSERT INTO ArtistGenre(IdArtist, IdGenre)
VALUES
    (1, 1)
,   (1, 2)
,   (1, 3)
,   (2, 1)
,   (2, 2)
,   (3, 1)
,   (3, 2)
,   (3, 3)
,   (3, 4)
,   (4, 1);