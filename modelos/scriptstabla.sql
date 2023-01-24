DELIMITER //
    CREATE TABLE ControlesMaestrosMultiples(
        CMM_ControlId BIGSERIAL NOT NULL,
        CMM_Control VARCHAR(255) NOT NULL,
        CMM_ValorES VARCHAR(255) NOT NULL,
        CMM_ValorEN VARCHAR(255) NOT NULL,
        CMM_Activo bit NOT NULL,
        CMM_USU_CreadoPorId INT NOT NULL,
        CMM_FechaCreacion datetime NOT NULL,
        CMM_USU_ModificadoPorId INT NULL,
        CMM_FechaModificacion datetime NULL,
        -- asociaciones 
        PRIMARY KEY (CMM_ControlId),
        CONSTRAINT FK_USUARIOCREADORID FOREIGN KEY (CMM_USU_CreadoPorId) REFERENCES seg_001_usuario (PK_UsuarioId)
                ON DELETE CASCADE
                ON UPDATE RESTRICT,
        CONSTRAINT FK_USUARIOMODIFICADOID FOREIGN KEY (CMM_USU_ModificadoPorId) REFERENCES seg_001_usuario (PK_UsuarioId)
                ON DELETE CASCADE
                ON UPDATE RESTRICT
    )
//
DELIMITTER;

DELIMITER //
    CREATE TABLE Roles(
        ROL_rolId BIGSERIAL NOT NULL,
        ROL_Nombre VARCHAR(100) NOT NULL,
        ROL_Activo bit NOT NULL,
        ROL_USU_UsuarioCreadorId INT NOT NULL,
        ROL_FechaCreacion datetime NOT NULL,
        ROL_USU_ModificadoPorId INT NULL,
        ROL_FechaModificacion datetime NULL,
        -- asociaciones 
        PRIMARY KEY (ROL_rolId),
        CONSTRAINT FK_USUARIOCREADORID_ROL FOREIGN KEY (ROL_USU_UsuarioCreadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
                ON DELETE CASCADE
                ON UPDATE RESTRICT,
        CONSTRAINT FK_USUARIOMODIFICADOID_ROL FOREIGN KEY (ROL_USU_ModificadoPorId) REFERENCES seg_001_usuario (PK_UsuarioId)
                ON DELETE CASCADE
                ON UPDATE RESTRICT
    )
//
DELIMITTER;

DELIMITER //
    CREATE TABLE Usuarios(
        USU_usuarioId BIGSERIAL NOT NULL,
        USU_Nombre VARCHAR(200) NOT NULL,
        USU_Contrasenia INT NOT NULL,
        USU_FechaUltimaSesion datetime NULL,
        USU_USU_CreadoPorId INT NOT NULL,
        USU_FechaCreacion datetime NOT NULL,
        USU_USU_ModificadoPorId INT NULL,
        USU_FechaModificacion datetime NULL,
        -- asociaciones 
        PRIMARY KEY (ROL_rolId),
        CONSTRAINT FK_USUARIOCREADORID_ROL FOREIGN KEY (ROL_USU_UsuarioCreadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
                ON DELETE CASCADE
                ON UPDATE RESTRICT,
        CONSTRAINT FK_USUARIOMODIFICADOID_ROL FOREIGN KEY (ROL_USU_ModificadoPorId) REFERENCES seg_001_usuario (PK_UsuarioId)
                ON DELETE CASCADE
                ON UPDATE RESTRICT
    )
//
DELIMITTER;