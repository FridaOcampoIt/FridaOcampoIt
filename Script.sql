SELECT *FROM cat_003_compania;
SELECT *FROM sis_024_controles_maestros_multiples;

   ------
   INSERT INTO sis_024_controles_maestros_multiples (
        ControlMaestroId,
        Nombre,
        Valor,
        FK_UsuarioCreador, 
        FechaCreacion,
        Activo,
        CmmSistema
    )
    VALUES 
    (
        1000022,
        'TIPO_GIRO_COMPANIA',
        'Reproceso',
        1,
        now(),
        true,
        true
    );
   
   INSERT INTO sis_024_controles_maestros_multiples (
        ControlMaestroId,
        Nombre,
        Valor,
        FK_UsuarioCreador, 
        FechaCreacion,
        Activo,
        CmmSistema
    )
    VALUES 
    (
        1000023,
        'TIPOGIROCOMPANIA',
        'No Reproceso',
        1,
        now(),
        true,
        true
    );
   
   INSERT INTO sis_024_controles_maestros_multiples (
        ControlMaestroId,
        Nombre,
        Valor,
        FK_UsuarioCreador, 
        FechaCreacion,
        Activo,
        CmmSistema
    )
    VALUES 
    (
        1000024,
        'TIPOGIROCOMPANIA',
        'Híbrido',
        1,
        now(),
        true,
        true
    );
-- columna tipo giro
ALTER TABLE cat_003_compania ADD Column FK_CMMTipoGiro Bigint UNSIGNED not null DEFAULT 1000022;

ALTER TABLE cat_003_compania  ADD CONSTRAINT FKTIPOGIRO
FOREIGN KEY ( FK_CMMTipoGiro) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID);
	

--SEGUNDA CLAVE FORACEA EN CONTROLES MAESTROS Y SU INSERCION DE DATOS

DELIMETER //
INSERT INTO sis_024_controles_maestros_multiples (
  ControlMaestroId,
  Nombre,
  Valor,
  FK_UsuarioCreador, 
  FechaCreacion,
  Activo,
  CmmSistema
)
VALUES 
  (
  1000025,
  'ESTATUS_COMPANIA',
  'ACTIVO',
  1,
  now(),
  true,
  true
);
//
DELIMETER;
DELIMETER //
INSERT INTO sis_024_controles_maestros_multiples (
  ControlMaestroId,
  Nombre,
  Valor,
  FK_UsuarioCreador, 
  FechaCreacion,
  Activo,
  CmmSistema
)
VALUES 
(
  1000026,
  'ESTATUS_COMPANIA',
  'INACTIVO',
  1,
  now(),
  true,
  true
  );
//
DELIMETER; 
DELIMETER //  
INSERT INTO sis_024_controles_maestros_multiples (
  ControlMaestroId,
  Nombre,
  Valor,
  FK_UsuarioCreador, 
  FechaCreacion,
  Activo,
  CmmSistema
)
VALUES 
(
  1000027,
  'ESTATUS_COMPANIA',
  'ELIMINADO',
  1,
  now(),
  true,
  true
);
// 
DELIMETER;

DELIMETER //
--AGREGAR TABLA A COMPAÑIA
ALTER TABLE cat_003_compania ADD Column FK_CMMEstatus Bigint UNSIGNED not null DEFAULT 1000025;
//
DELIMETER;
--CRER LA CLAVE FORANEA DE COMPAÑIA A CONTROLES MAESTROS
DELIMETER //
ALTER TABLE cat_003_compania  ADD CONSTRAINT FKESTATUS
FOREIGN KEY ( FK_CMMEstatus) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID);
//
DELIMETER;

-- -----------------------------pruebas----------------------------------------------------------
ALTER TABLE cat_003_compania DROP FOREIGN KEY FKESTATUS ; 
ALTER TABLE cat_003_compania DROP COLUMN FK_CMMestatus;

SELECT 	CompaniaId as idCompany,
			Nombre AS name,
			RazonSocial AS businessName,
            Telefono AS phone
		FROM cat_003_compania
	WHERE Nombre LIKE CONCAT("%", _name, "%") AND
		  RazonSocial LIKE CONCAT("%", _businessName, "%") and FK_CMMEstatus=1000025 OR FK_CMMEstatus=1000026  /*muestra las compañias activas*/
	ORDER BY name;
END
select CompaniaId, Nombre, RazonSocial FROM cat_003_compania where FK_CMMEstatus = 1000025 OR FK_CMMEstatus=1000026 ORDER BY Nombre;


-- Formulario
SELECT *FROM rel_054_formulario_sector rfs 
  

DELIMITER //
-- ****************TABLA 1*********
	CREATE TABLE cat_052_sectores(
	    PK_sectorId BIGINT UNSIGNED AUTO_INCREMENT NOT NULL,
	    Nombre varchar(200),
	    DescripcionCorta varchar(500),
	    FK_UsuarioCreadorId INT (11) NOT NULL,
	    FechaCreacion DATETIME DEFAULT NOW() NOT NULL ,
	    FK_usuarioModificadorId INT (11),
	    FechaModificacion DATETIME,
	    FK_CMMEstatusId BIGINT UNSIGNED DEFAULT 1000038 NOT NULL,
		-- asociaciones 
		PRIMARY KEY (PK_sectorId),
		CONSTRAINT FK_USUARIOCREADORID_SECTOR FOREIGN KEY (FK_UsuarioCreadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_USUARIOMODIFICADORID_SECTOR FOREIGN KEY (FK_usuarioModificadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_CMMESTATUS_ID_SECTOR FOREIGN KEY (FK_CMMEstatusId) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT
	)
//
DELIMITER;

DELIMITER //
-- ****************TABLA 2*********
	CREATE TABLE cat_053_formulario(
	    PK_FormularioId BIGINT UNSIGNED AUTO_INCREMENT NOT NULL,
		Nombre varchar(200), 
		DescripcionCorta varchar(500),
		FK_UsuarioCreadorId INT (11) NOT NULL, 
		FechaCreacion DATETIME DEFAULT NOW() NOT NULL,
		FK_UsuarioModificadorId INT (11),
		FechaModificacion DATETIME,
		FK_CMMEstatusFormularioId BIGINT UNSIGNED DEFAULT 1000041 NOT NULL,
		-- asociaciones 
		PRIMARY KEY (PK_FormularioId),
		CONSTRAINT FK_USUARIOCREADORID_FORMULARIO FOREIGN KEY (FK_UsuarioCreadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_USUARIOMODIFICADOID_FORMULARIO FOREIGN KEY (FK_usuarioModificadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_CMMESTATUSFORMULARIOID_FORMULARIO FOREIGN KEY (FK_CMMEstatusFormularioId) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT
	)
//
DELIMITER;

DELIMITER //
-- ****************TABLA 3*********
	CREATE TABLE rel_054_formulario_sector(
	    PK_FormularioSectorId BIGINT UNSIGNED AUTO_INCREMENT NOT NULL ,
		FK_SectorId BIGINT UNSIGNED,
		FK_FormularioId BIGINT UNSIGNED,
			-- asociaciones 
		PRIMARY KEY (PK_FormularioSectorId),
		CONSTRAINT FK_SECTORID_SECTOR FOREIGN KEY (FK_SectorId) REFERENCES cat_052_sectores (PK_sectorId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_FORMULARIOID_FORMULARIO_SECTOR FOREIGN KEY (FK_FormularioId) REFERENCES cat_053_formulario (PK_FormularioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT
	)
//
DELIMITER;

DELIMITER //
-- SPI GUARDAR SECTORES TAMBLA 1
	DROP PROCEDURE IF EXISTS spi_guardarSectores;
    CREATE PROCEDURE spi_guardarSectores(
        IN _Nombre varchar(200),
        IN _DescripcionCorta varchar(500),
        IN _UsuarioCreadorId INT (11),
        INOUT _response int
    )
    BEGIN
        /*
            Proyecto: TraceIt
            Programador: Roberto Ortega
            Creación: 25-Nov-2022
            Descripción: SP utilizado para guardar un sector
            Querido  programador corre.
        */
        INSERT INTO cat_052_sectores 
            (
             Nombre,
             DescripcionCorta,
             FK_UsuarioCreadorId
            )
            VALUES
            (
             _Nombre, 
             _DescripcionCorta,
             _UsuarioCreadorId
            );
            -- Regresamos el ultimo id insertado. 
            -- (No es necesario pero lo regresamos)
        SET _response =  last_insert_id();
    END
//
DELIMITER;

DELIMITER //
DROP PROCEDURE IF EXISTS spc_consultarSectores;
// 
DELIMITER;

DELIMITER //
-- SPC CONSULTA SECTORES TABLA 1
 CREATE PROCEDURE spc_consultarSectores()        
    BEGIN
        SELECT 
            sec.PK_sectorId  sectorId,
            sec.Nombre  nombre,
            sec.DescripcionCorta descripcionCorta,
            sec.FK_CMMEstatusId  EstatusId,
            sec.FK_UsuarioCreadorId UsuarioCreadorId,
            sec.FK_usuarioModificadorId usuarioModificadorId,
            estado.Valor  Valor,
            CONCAT(usu.Nombre , ' ', usu.Apellido)  nombreCompleto
            FROM cat_052_sectores sec
            INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId  = sec.FK_CMMEstatusId  
            INNER JOIN seg_001_usuario usu ON usu.PK_UsuarioId  = sec.FK_UsuarioCreadorId
            WHERE estado.Valor="ACTIVO" OR estado.Valor="INACTIVO";
    END
//
DELIMITER;

DELIMITER //
	DROP PROCEDURE IF EXISTS spd_eliminarSector;
// 
DELIMITER:
DELIMITER //
-- SPC ELIMINAR SECTORES TABLA 1
    CREATE PROCEDURE spd_eliminarSector(
        IN _sectorId INT(11),
        INOUT _response int
    )
    BEGIN
        UPDATE cat_052_sectores set
        FK_CMMEstatusId = 1000040 WHERE 
        PK_sectorId = _sectorId;
		set _response= 1;
	END
//
DELIMITER;

DELIMITER //
	DROP PROCEDURE IF EXISTS spu_editarSector;
//
DELIMITER;

DELIMITER //
-- SPC ACTUALIZAR SECTORES TABLA 1
    CREATE PROCEDURE spu_editarSector(
        IN _sectorId INT(11),
        IN _nombre VARCHAR(200),
        IN _usuarioModificadorId INT (11),
        IN _descripcionCorta VARCHAR(200),
        IN _estatusId BIGINT,
        INOUT _response int
    )
    BEGIN
        UPDATE cat_052_sectores  sec 
        INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId = sec.FK_CMMEstatusId 
		SET sec.Nombre = _nombre,
			sec.FK_usuarioModificadorId =_usuarioModificadorId,
			sec.DescripcionCorta =_descripcionCorta,
			sec.FK_CMMEstatusId =_estatusId,
			sec.FechaModificacion = NOW()
		WHERE  PK_sectorId  = _sectorId;
	SET _response = 1;
	END	
	
//
DELIMITER;

DELIMITER //
	DROP PROCEDURE IF EXISTS spc_consultarSectoresById;
// 
DELIMITER;

DELIMITER //
-- SPC CONSULTA SECTORES BY ID TABLA 1
 CREATE PROCEDURE spc_consultarSectoresById(
 	IN _sectorId INT (11)
 )        
    BEGIN
        SELECT 
        	sec.PK_sectorId  sectorId,
            sec.Nombre  nombre,
            sec.DescripcionCorta descripcionCorta,
            sec.FK_CMMEstatusId  EstatusId,
            sec.FK_UsuarioCreadorId UsuarioCreadorId,
            sec.FK_usuarioModificadorId usuarioModificadorId,
            estado.Valor  Valor,
            CONCAT(usu.Nombre , ' ', usu.Apellido)  nombreCompleto
            FROM cat_052_sectores sec
            INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId  = sec.FK_CMMEstatusId  
            INNER JOIN seg_001_usuario usu ON usu.PK_UsuarioId  = sec.FK_UsuarioCreadorId 
            WHERE sec.PK_sectorId = _sectorId;
    END
//
DELIMITER;

-- ******************************tabla 2*************

SELECT *FROM rel_054_formulario_sector 
SELECT *FROM cat_052_sectores cs  
SELECT *FROM cat_053_formulario cf 




DELIMITER //
	DROP PROCEDURE IF EXISTS spc_consultarFormulario;
// 
DELIMITER;

DELIMITER //
-- SPI GUARDAR FORMULARIO TABLA 2
        CREATE PROCEDURE spi_guardarFormulario(
        IN _Nombre varchar(200),
        IN _DescripcionCorta varchar(500),
        IN _UsuarioCreadorId INT (11),
        INOUT _response int
    )
    BEGIN
        /*
            Proyecto: TraceIt
            Programador: Roberto Ortega
            Creación: 25-Nov-2022
            Descripción: SP utilizado para guardar un formulario
            Querido  programador corre.
        */
        INSERT INTO cat_053_formulario 
            (
             Nombre,
             DescripcionCorta,
             FK_UsuarioCreadorId
            )
            VALUES
            (
             _Nombre, 
             _DescripcionCorta,
             _UsuarioCreadorId
            );
            -- Regresamos el ultimo id insertado. 
            -- (No es necesario pero lo regresamos)
        SET _response =  last_insert_id();
    END
//
DELIMITER;

DELIMITER //
-- SPC CONSULTA formulario TABLA 2
 CREATE PROCEDURE spc_consultarFormulario(
 IN _sectorId INT (11))        
    BEGIN
        SELECT 
            form.PK_FormularioId formularioId,
            form.Nombre nombre,
            form.DescripcionCorta DescripcionCorta,
            form.FK_CMMEstatusFormularioId  EstatusFormularioId,
            form.FK_UsuarioCreadorId usuarioCreadorId,
            form.FK_UsuarioModificadorId UsuarioModificadorId,
            estado.Valor  valorestus,
            fs.FK_SectorId  sectorId,
            CONCAT(usu.Nombre , ' ', usu.Apellido)  nombreCompleto
            FROM cat_053_formulario form 
            INNER JOIN sis_024_controles_maestros_multiples estado ON  estado.ControlMaestroId  = form.FK_CMMEstatusFormularioId 
            INNER JOIN seg_001_usuario usu ON usu.PK_UsuarioId = form.FK_UsuarioCreadorId	
            INNER JOIN rel_054_formulario_sector fs ON fs.FK_FormularioId  = form.PK_FormularioId 
            WHERE estado.Valor="ACTIVO" OR estado.Valor="INACTIVO"  ;
//
DELIMITER;

DELIMITER //
-- SPC ACTUALIZAR SECTORES TABLA 2
    CREATE PROCEDURE spu_editarFormulario(
        IN _formularioId INT(11),
        IN _nombre VARCHAR(200),
        IN _usuarioModificadorId INT (11),
        IN _descripcionCorta VARCHAR(200),
        IN _estatusId BIGINT,
        INOUT _response int
    )
    BEGIN
        UPDATE cat_053_formulario  form 
        INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId = form.FK_CMMEstatusFormularioId 
		SET form.Nombre  = _nombre,
			form.FK_UsuarioModificadorId  =_usuarioModificadorId,
			form.DescripcionCorta  =_descripcionCorta,
			form.FK_CMMEstatusFormularioId  =_estatusId,
			form.FechaModificacion = NOW()
		WHERE  PK_FormularioId  = _formularioId;
	SET _response = 1;
	END	
	
//
DELIMITER;

DELIMITER //
-- SPC CONSULTA SECTORES BY ID TABLA 1
 CREATE PROCEDURE spc_consultarFormulariosById(
 	IN _formularioId INT (11)
 )        
    BEGIN
        SELECT 
        	form.PK_FormularioId  formularioId,
            form.Nombre  nombre,
            form.DescripcionCorta descripcionCorta,
            form.FK_CMMEstatusFormularioId  EstatusId,
            form.FK_UsuarioCreadorId  UsuarioCreadorId,
            form.FK_UsuarioModificadorId usuarioModificadorId,
            estado.Valor  Valor,
            CONCAT(usu.Nombre , ' ', usu.Apellido)  nombreCompleto
            FROM cat_053_formulario form
            INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId  = form.FK_CMMEstatusFormularioId  
            INNER JOIN seg_001_usuario usu ON usu.PK_UsuarioId  = form.FK_UsuarioCreadorId  
            WHERE form.PK_FormularioId = _formularioId;
    END
//
DELIMITER;

SELECT *FROM rel_054_formulario_sector 
SELECT *FROM cat_052_sectores cs  
SELECT *FROM cat_053_formulario cf 

DELIMITER //
	DROP PROCEDURE IF EXISTS spi_guardarDuplicadoFormulario;
//
DELIMITER;

DeLIMITER //
-- SPI GENRAR UN DUPLICADO 
 CREATE PROCEDURE spi_guardarDuplicadoFormulario(
 	IN _formularioId INT (11)
 )        
    BEGIN
        INSERT INTO cat_053_formulario  (
			Nombre, 
			DescripcionCorta,
			FK_UsuarioCreadorId,
			FechaCreacion,
			FK_UsuarioModificadorId,
			FechaModificacion,
			FK_CMMEstatusFormularioId)
	SELECT 	
			form.Nombre, 
			form.DescripcionCorta,
			form.FK_UsuarioCreadorId,
			form.FechaCreacion,
			form.FK_UsuarioModificadorId,
			form.FechaModificacion,
			form.FK_CMMEstatusFormularioId,
			fs.FK_SectorId 
			FROM cat_053_formulario form
			INNER JOIN rel_054_formulario_sector fs on fs.FK_FormularioId = form.PK_FormularioId 
			WHERE form.PK_FormularioId =_formularioId;
    END
//
DELIMITER;


DELIMITER //
//
DELIMITTER;

