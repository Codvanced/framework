/*-------------------------------------------------------------------------------------------------------------*/
/*-------------------------- Script para Inserção da Base de Dados do Codvanced Framework (IOC FW) ------------*/
/*-------------------------------------------------------------------------------------------------------------*/

/* --- INSERE OS REGISTROS --- */
INSERT INTO Ocupation (OcupationName) VALUES ('Ocupation 1');
INSERT INTO Ocupation (OcupationName) VALUES ('Ocupation 2');
INSERT INTO Ocupation (OcupationName) VALUES ('Ocupation 3');

INSERT INTO Person (PersonName, IdOcupation, Gender) VALUES ('Person 1', 1, 'Masculino');
INSERT INTO Person (PersonName, IdOcupation, Gender) VALUES ('Person 2', 2, 'Feminino');
INSERT INTO Person (PersonName, IdOcupation, Gender) VALUES ('Person 3', 3, 'Masculino');

INSERT INTO Artist (Name) VALUES ('Artista1');
INSERT INTO Artist (Name) VALUES ('Artista2');
INSERT INTO Artist (Name) VALUES ('Artista3');
INSERT INTO Artist (Name) VALUES ('Artista4');
INSERT INTO Artist (Name) VALUES ('Artista5');

INSERT INTO Genre (Name) VALUES ('Genre1');
INSERT INTO Genre (Name) VALUES ('Genre2');
INSERT INTO Genre (Name) VALUES ('Genre3');
INSERT INTO Genre (Name) VALUES ('Genre4');

INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (1, 1);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (1, 2);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (1, 3);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (2, 1);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (2, 2);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (3, 1);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (3, 2);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (3, 3);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (3, 4);
INSERT INTO ArtistGenre(IdArtist, IdGenre) VALUES (4, 1);