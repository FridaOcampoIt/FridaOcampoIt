SELECT *FROM cat_003_compania;
SELECT *FROM sis_024_controles_maestros_multiples;
-- TIPO DE GIRO -----------------------------------
DELIMITER //
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
//
DELIMITER;
   
DELIMITER //
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
   //
DELIMITER;
DELIMITER //
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
   //
DELIMITER;
   
-- COLUMNA DE TIPO DE GIRO -------------------------
ALTER TABLE cat_003_compania ADD Column FK_CMMTipoGiro Bigint UNSIGNED not null DEFAULT 1000022;

ALTER TABLE cat_003_compania  ADD CONSTRAINT FKTIPOGIRO
FOREIGN KEY ( FK_CMMTipoGiro) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID);
	

-- SEGUNDA CLAVE FORANEA EN CONTROLES MAESTROS Y SU INSERCION DE DATOS
DELIMITER //
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
DELIMITER;
DELIMITER //
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
DELIMITER; 
DELIMITER //  
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
DELIMITER;

DELIMITER //
-- AGREGAR TABLA A COMPAÑIA
ALTER TABLE cat_003_compania ADD Column FK_CMMEstatus Bigint UNSIGNED not null DEFAULT 1000025;
//
DELIMITER;
--CRER LA CLAVE FORANEA DE COMPAÑIA A CONTROLES MAESTROS
DELIMITER //
ALTER TABLE cat_003_compania  ADD CONSTRAINT FKESTATUS
FOREIGN KEY ( FK_CMMEstatus) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID);
//
DELIMITER;

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
  
-- ****************TABLA 1*********
DELIMITER //
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
-- ****************TABLA 2*********
DELIMITER //
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
-- ****************TABLA 3*********
DELIMITER //
	CREATE TABLE rel_054_formulario_sector(
	    PK_FormularioSectorId BIGINT UNSIGNED AUTO_INCREMENT NOT NULL ,
		FK_SectorId BIGINT UNSIGNED,
		FK_FormularioId BIGINT UNSIGNED, 
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
-- SPI GUARDAR SECTORES TAMBLA 1 -----------------
DELIMITER //
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
-- SPC CONSULTA SECTORES TABLA 1 ------------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spc_consultarSectores;
// 
DELIMITER;
DELIMITER //
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
-- SPC ELIMINAR SECTORES TABLA 1 -------------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spd_eliminarSector;
// 
DELIMITER:
DELIMITER //
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
-- SPC ACTUALIZAR SECTORES TABLA 1 -----------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spu_editarSector;
//
DELIMITER;
DELIMITER //
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
-- SPC CONSULTA SECTORES BY ID TABLA 1 -------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spc_consultarSectoresById;
// 
DELIMITER;
DELIMITER //
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



-- SPI GUARDAR FORMULARIO TABLA 2 ------------------
DELIMITER //
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
-- SPC CONSULTA formulario TABLA 2 -----------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spc_consultarFormulario;
// 
DELIMITER;
DELIMITER //
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
-- SPC ACTUALIZAR SECTORES TABLA 2 -----------------
DELIMITER //
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
-- SPC CONSULTA SECTORES BY ID TABLA 1 -------------
DELIMITER //
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
SELECT *FROM cat_054_categoria cc 
SELECT *FROM cat_055_pregunta cp  
SELECT *FROM cat_056_propiedad cp 
-- SPI GENRAR UN DUPLICADO --------------------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spi_guardarDuplicadoFormulario;
//
DELIMITER;

DeLIMITER // 
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
			CONCAT(form.Nombre,'-Copy'),
			form.DescripcionCorta,
			form.FK_UsuarioCreadorId,
			form.FechaCreacion,
			form.FK_UsuarioModificadorId,
			form.FechaModificacion, -- usuario clonado
			form.FK_CMMEstatusFormularioId,
			fs.FK_SectorId 
			FROM cat_053_formulario form
			INNER JOIN rel_054_formulario_sector fs on fs.FK_FormularioId = form.PK_FormularioId 
			WHERE form.PK_FormularioId =_formularioId;
    END
//
DELIMITER;

-- -------------------------------- TABLAS DE FORMULARIO ---------------------------------------------------------------------------
-- CATEGORIAS --------------------------------------
DELIMITER //
	CREATE TABLE cat_054_categoria(
	    PK_CategoriaId BIGINT UNSIGNED AUTO_INCREMENT NOT NULL,
        FK_FormularioId BIGINT UNSIGNED,
		Nombre varchar(200), 
		DescripcionCorta varchar(500),
		FK_UsuarioCreadorId INT (11) NOT NULL, 
		FechaCreacion DATETIME DEFAULT NOW() NOT NULL,
		FK_UsuarioModificadorId INT (11),
		FechaModificacion DATETIME,
		FK_CMMEstatusCategoriaId BIGINT UNSIGNED DEFAULT 1000050 NOT NULL,
        Tipo varchar(100),
		-- asociaciones 
		PRIMARY KEY (PK_CategoriaId),
		CONSTRAINT FK_USUARIOCREADORID_CATEGORIA FOREIGN KEY (FK_UsuarioCreadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_USUARIOMODIFICADOID_CATEGORIA FOREIGN KEY (FK_usuarioModificadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_CMMESTATUSCATEGORIAID_CATEGORIA FOREIGN KEY (FK_CMMEstatusCategoriaId) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
        CONSTRAINT FK_FORMULARIOID_FORMULARIO_CATEGORIA FOREIGN KEY (FK_FormularioId) REFERENCES cat_053_formulario (PK_FormularioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT
	)
//
DELIMITTER;
-- PREGUNTAS ---------------------------------------
DELIMITER //
	CREATE TABLE cat_055_pregunta(
	    PK_PreguntaId BIGINT UNSIGNED AUTO_INCREMENT NOT NULL,
        FK_CategoriaId BIGINT UNSIGNED,
		Nombre varchar(200),
        Placeholder varchar (50), 
		FK_UsuarioCreadorId INT (11) NOT NULL, 
		FK_UsuarioModificadorId INT (11),
		FechaCreacion DATETIME DEFAULT NOW() NOT NULL,
		FechaModificacion DATETIME,
		FK_CMMEstatusPreguntaId BIGINT UNSIGNED DEFAULT 1000053 NOT NULL,
        TipoPregunta varchar(50),
		-- asociaciones 
		PRIMARY KEY (PK_PreguntaId),
		CONSTRAINT FK_USUARIOCREADORID_PREGUNTA FOREIGN KEY (FK_UsuarioCreadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_USUARIOMODIFICADOID_PREGUNTA FOREIGN KEY (FK_usuarioModificadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
		CONSTRAINT FK_CMMESTATUSPREGUNTAID_PREGUNTA FOREIGN KEY (FK_CMMEstatusPreguntaId) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT,
        CONSTRAINT FK_FORMULARIOID_FORMULARIO_PREGUNTA FOREIGN KEY (FK_CategoriaId) REFERENCES cat_054_categoria (PK_CategoriaId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT
	)
//
DELIMITTER;

-- PROPIEDADES --------------------------------------
	DELIMITER //
		CREATE TABLE cat_056_propiedad(
		    PK_PropiedadId BIGINT UNSIGNED AUTO_INCREMENT NOT NULL,
		    FK_PreguntaId BIGINT UNSIGNED,
			Nombre varchar(200), 
			Valor INT (11),
			FK_UsuarioCreadorId INT (11) NOT NULL, 
			FechaCreacion DATETIME DEFAULT NOW() NOT NULL,
			FK_UsuarioModificadorId INT (11),
			FechaModificacion DATETIME,
			FK_CMMEstatusPropiedadId BIGINT UNSIGNED DEFAULT 1000056 NOT NULL,
			-- asociaciones 
			PRIMARY KEY (PK_PropiedadId),
			CONSTRAINT FK_USUARIOCREADORID_PROPIEDAD FOREIGN KEY (FK_UsuarioCreadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
		            ON DELETE CASCADE
		            ON UPDATE RESTRICT,
			CONSTRAINT FK_USUARIOMODIFICADOID_PROPIEDAD FOREIGN KEY (FK_usuarioModificadorId) REFERENCES seg_001_usuario (PK_UsuarioId)
		            ON DELETE CASCADE
		            ON UPDATE RESTRICT,
			CONSTRAINT FK_CMMESTATUSPROPIEDADID_PROPIEDAD FOREIGN KEY (FK_CMMEstatusPropiedadId) REFERENCES sis_024_controles_maestros_multiples (ControlMaestroID)
		            ON DELETE CASCADE
		            ON UPDATE RESTRICT,
		    CONSTRAINT FK_FORMULARIOID_FORMULARIO_PREGUNTA_ID FOREIGN KEY (FK_PreguntaId) REFERENCES cat_055_pregunta (PK_PreguntaId)
	            ON DELETE CASCADE
	            ON UPDATE RESTRICT
		)
	//
	DELIMITTER;
-- ----------------------------------------SP -------------------------------------------------------------------------------------- 
-- SPC ACTUALIZAR CATEGORIA -------------------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spu_editarCategoria;
//
DELIMITER;
DELIMITER //
    CREATE PROCEDURE spu_editarCategoria(
        IN _categoriaId INT(11),
        IN _nombre VARCHAR(200),
        IN _usuarioModificadorId INT (11),
        IN _descripcionCorta VARCHAR(200),
        IN _estatusCategoriaId BIGINT,
        IN _tipo VARCHAR(50),
        INOUT _response int
    )
    BEGIN
        UPDATE cat_054_categoria  fcat 
        INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId = fcat.FK_CMMEstatusCategoriaId 
		SET fcat.Nombre  = _nombre,
			fcat.FK_UsuarioModificadorId  =_usuarioModificadorId,
			fcat.DescripcionCorta  =_descripcionCorta,
			fcat.FK_CMMEstatusCategoriaId  =_estatusCategoriaId,
			fcat.FechaModificacion = NOW()
		WHERE  PK_CategoriaId  = _categoriaId;
	SET _response = 1;
	END	
//
DELIMITER;
-- SPC ACTUALIZAR PREGUNTA --------------------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spu_editarPregunta;
//
DELIMITER;
DELIMITER //
    CREATE PROCEDURE spu_editarPregunta(
        IN _preguntaId INT(11),
        IN _nombre VARCHAR(200),
        IN _placeholder VARCHAR(50),
        IN _usuarioModificadorId INT (11),
        IN _tipoPregunta VARCHAR(50),
        IN _estatusPreguntaId BIGINT,
        INOUT _response int
    )
    BEGIN
        UPDATE cat_055_pregunta  pre
        INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId = pre.FK_CMMEstatusPreguntaId 
		SET pre.Nombre  = _nombre,
			pre.FK_UsuarioModificadorId  =_usuarioModificadorId,
			pre.TipoPregunta  =_tipoPregunta,
			pre.FK_CMMEstatusPreguntaId  =_estatusPreguntaId,
			pre.Placeholder  =_placeholder,
			pre.FechaModificacion = NOW()
		WHERE  PK_PreguntaId  = _preguntaId;
	SET _response = 1;
	END	
//
DELIMITER;
-- SPC ACTUALIZAR PROPIEDAD --------------------------
DELIMITER //
	DROP PROCEDURE IF EXISTS spu_editarPropiedad;
//
DELIMITER;
DELIMITER //
    CREATE PROCEDURE spu_editarPropiedad(
        IN _propiedadId INT(11),
        IN _nombre VARCHAR(200),
        IN _usuarioModificadorId INT (11),
        IN _valor INT(11),
        IN _estatusPropiedadId BIGINT,
        INOUT _response int
    )
    BEGIN
        UPDATE cat_056_propiedad  pro 
        INNER JOIN sis_024_controles_maestros_multiples estado ON estado.ControlMaestroId = pro.FK_CMMEstatusCategoriaId 
		SET pro.Nombre  = _nombre,
			pro.FK_UsuarioModificadorId  =_usuarioModificadorId,
			pro.Valor  =_valor,
			pro.FK_CMMEstatusCategoriaId  =_estatusPropiedadId,
			pro.FechaModificacion = NOW()
		WHERE  PK_PropiedadId  = _propiedadId;
	SET _response = 1;
	END		
//
DELIMITER;
-- --------------------------------------INSERTO TABLA MAESTRA------------------------------------------------------------------------
-- CATEGORIA -----------------------------------------
DELIMITER //  
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
	  1000050,
	  'ESTATUS_CATEGORIA',
	  'ACTIVO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

DELIMITER //  
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
	  1000051,
	  'ESTATUS_CATEGORIA',
	  'INACTIVO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

DELIMITER //  
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
	  1000052,
	  'ESTATUS_CATEGORIA',
	  'ELIMINADO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

-- PREGUNTA ------------------------------------------
DELIMITER //  
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
	  1000053,
	  'ESTATUS_PREGUNTA',
	  'ACTIVO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

DELIMITER //  
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
	  1000054,
	  'ESTATUS_PREGUNTA',
	  'INACTIVO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

DELIMITER //  
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
	  1000055,
	  'ESTATUS_PREGUNTA',
	  'ELIMINADO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

-- PROPIEDADES ---------------------------------------
DELIMITER //  
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
	  1000056,
	  'ESTATUS_PROPIEDADES',
	  'ACTIVO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

DELIMITER //  
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
	  1000057,
	  'ESTATUS_PROPIEDADES',
	  'INACTIVO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;

DELIMITER //  
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
	  1000058,
	  'ESTATUS_PROPIEDADES',
	  'ELIMINADO',
	  1,
	  now(),
	  true,
	  true
	);
// 
DELIMITER;
