-- Primero crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'persona_db')
BEGIN
    CREATE DATABASE persona_db;
END
GO

-- Asignar la base de datos al usuario 'sa'
ALTER AUTHORIZATION ON DATABASE::persona_db TO sa;
GO

-- Usar la base de datos
USE persona_db;
GO

-- Tabla persona
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'persona')
BEGIN
    CREATE TABLE persona (
        cc INT NOT NULL,
        nombre VARCHAR(45) NOT NULL,
        apellido VARCHAR(45) NOT NULL,
        edad INT NULL,
        genero CHAR(1) NOT NULL,
        CONSTRAINT PK_persona_3213666D564B0149 PRIMARY KEY (cc)
    );
END
GO

-- Tabla profesion
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'profesion')
BEGIN
    CREATE TABLE profesion (
        id INT NOT NULL,
        nom VARCHAR(90) NOT NULL,
        des TEXT NOT NULL,
        CONSTRAINT PK_profesio_3213E83F2B1F13F8 PRIMARY KEY (id)
    );
END
GO

-- Tabla telefono
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'telefono')
BEGIN
    CREATE TABLE telefono (
        num VARCHAR(15) NOT NULL,
        oper VARCHAR(45) NOT NULL,
        duenio INT NOT NULL,
        CONSTRAINT PK_telefono_DF908D659694C3B2 PRIMARY KEY (num),
        CONSTRAINT FK_telefonoduenio_37A5467C FOREIGN KEY (duenio)
            REFERENCES persona (cc)
    );
END
GO

-- Tabla estudios
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'estudios')
BEGIN
    CREATE TABLE estudios (
        id_prof INT NOT NULL,
        cc_per INT NOT NULL,
        fecha DATETIME2 NULL,
        univer VARCHAR(50) NOT NULL,
        PersonaCc INT NOT NULL,
        ProfesionId INT NOT NULL,
        CONSTRAINT PK_estudios_FB3F71A6B8574F61 PRIMARY KEY (id_prof, cc_per),
        CONSTRAINT FK_estudioscc_per_33D4B598 FOREIGN KEY (cc_per)
            REFERENCES persona (cc),
        CONSTRAINT FK_estudiosid_pro_34C8D9D1 FOREIGN KEY (id_prof)
            REFERENCES profesion (id),
        CONSTRAINT FK_estudios_persona_PersonaCc FOREIGN KEY (PersonaCc)
            REFERENCES persona (cc) ON DELETE CASCADE,
        CONSTRAINT FK_estudios_profesion_ProfesionId FOREIGN KEY (ProfesionId)
            REFERENCES profesion (id) ON DELETE CASCADE
    );
END
GO

-- Creación de índices
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_estudios_cc_per')
    CREATE INDEX IX_estudios_cc_per ON estudios (cc_per);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_estudios_PersonaCc')
    CREATE INDEX IX_estudios_PersonaCc ON estudios (PersonaCc);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_estudios_ProfesionId')
    CREATE INDEX IX_estudios_ProfesionId ON estudios (ProfesionId);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_telefono_duenio')
    CREATE INDEX IX_telefono_duenio ON telefono (duenio);
GO