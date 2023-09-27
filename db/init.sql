SET timezone = 'Europe/Paris';

----------- CITIES -----------

CREATE SEQUENCE CitiesIdSeq;

CREATE TABLE Cities (
    Id          INTEGER NOT NULL DEFAULT nextval('CitiesIdSeq'),
    Timestamp   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Name        VARCHAR(255) NOT NULL,
    Touristic   BOOLEAN NOT NULL,
    PRIMARY KEY(Id)
);

ALTER SEQUENCE CitiesIdSeq
OWNED BY Cities.id;

INSERT INTO Cities (Name, Touristic) VALUES ('Paris', true);
INSERT INTO Cities (Name, Touristic) VALUES ('New York', true);
INSERT INTO Cities (Name, Touristic) VALUES ('London', true);
INSERT INTO Cities (Name, Touristic) VALUES ('Bangkok', true);

----------- HOUSES -----------

CREATE SEQUENCE HousesIdSeq;

CREATE TABLE Houses (
    Id          INTEGER NOT NULL DEFAULT nextval('HousesIdSeq'),
    Timestamp   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Address     VARCHAR(255) NOT NULL,
    Cityid      INTEGER REFERENCES Cities (Id),
    Inhabitants INTEGER NOT NULL,
    PRIMARY KEY(Id)
);

ALTER SEQUENCE HousesIdSeq
OWNED BY Houses.id;

INSERT INTO Houses (Address, Cityid, Inhabitants) VALUES ('5 avenue Anatole France', 1, 100);
INSERT INTO Houses (Address, Cityid, Inhabitants) VALUES ('350 5th avenue', 2, 1000);
INSERT INTO Houses (Address, Cityid, Inhabitants) VALUES ('SW1A 0AA', 3, 500);
INSERT INTO Houses (Address, Cityid, Inhabitants) VALUES ('Phra Borom Maha Ratchawang', 4, 10);

----------- STORES -----------

CREATE SEQUENCE StoresIdSeq;

CREATE TABLE Stores (
    Id          INTEGER NOT NULL DEFAULT nextval('StoresIdSeq'),
    Timestamp   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Name        VARCHAR(255) NOT NULL,
    Type        VARCHAR(255) NOT NULL,
    Address     VARCHAR(255) NOT NULL,
    Cityid      INTEGER REFERENCES Cities (Id),
    PRIMARY KEY(Id)
);

ALTER SEQUENCE StoresIdSeq
OWNED BY Stores.id;

INSERT INTO Stores (Name, Type, Address, Cityid) VALUES ('Galleries Lafayettes', 'Clothing', '40 Boulevard Haussmann', 1);
INSERT INTO Stores (Name, Type, Address, Cityid) VALUES ('NBA Store', 'Sports', '545 5th avenue', 2);
INSERT INTO Stores (Name, Type, Address, Cityid) VALUES ('M&Ms London', 'Food', '1 Swiss court', 3);
INSERT INTO Stores (Name, Type, Address, Cityid) VALUES ('Gourmet Market', 'Food', 'Siam Paragon, 991 Rama', 4);
