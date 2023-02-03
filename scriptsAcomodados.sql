-- 1
DELIMITER //
CREATE TABLE Usuarios(
	usu_usuarioid BIGSERIAL NOT NULL,
	usu_nombre VARCHAR(200) NOT NULL,
	usu_contrasenia VARCHAR(200) NOT NULL,
	usu_fechaultimasesion timestamp NULL,
	usu_usu_creadoporid INT NOT NULL,
	usu_fechacreacion timestamp,
	usu_usu_modificadoporid INT NULL,
	usu_fechamodificacion timestamp,
	-- asociaciones 
	PRIMARY KEY (usu_usuarioid)
)
//
DELIMITTER;

-- 2
DELIMITER //
CREATE TABLE ControlesMaestrosMultiples(
	CMM_ControlId BIGSERIAL NOT NULL,
	CMM_Control VARCHAR(255) NOT NULL,
	CMM_ValorES VARCHAR(255) NOT NULL,
	CMM_ValorEN VARCHAR(255) NOT NULL,
	CMM_Activo bit NOT NULL,
	CMM_USU_CreadoPorId INT NOT NULL,
	CMM_FechaCreacion timestamp NOT NULL,
	CMM_USU_ModificadoPorId INT NULL,
	CMM_FechaModificacion timestamp NULL,
	-- asociaciones 
	PRIMARY KEY (CMM_ControlId),
	CONSTRAINT FK_USUARIOCREADORID FOREIGN KEY (CMM_USU_CreadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID FOREIGN KEY (CMM_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;

-- 3
DELIMITER //
CREATE TABLE Roles(
	ROL_rolId BIGSERIAL NOT NULL,
	ROL_Nombre VARCHAR(100) NOT NULL,
	ROL_Activo bit NOT NULL,
	ROL_USU_UsuarioCreadorId INT NOT NULL,
	ROL_FechaCreacion timestamp NOT NULL,
	ROL_USU_ModificadoPorId INT NULL,
	ROL_FechaModificacion timestamp NULL,
	-- asociaciones 
	PRIMARY KEY (ROL_rolId),
	CONSTRAINT FK_USUARIOCREADORID_ROL FOREIGN KEY (ROL_USU_UsuarioCreadorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_ROL FOREIGN KEY (ROL_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 4
DELIMITTER //
CREATE TABLE Formularios(
	FOR_FormularioId BIGSERIAL NOT NULL,
	FOR_Nombre VARCHAR(200) NOT NULL,
	FOR_CMM_EstatusId INT NOT NULL,
	FOR_USU_CreadoPorId INT NOT NULL,
	FOR_FechaCreacion timestamp NOT NULL,
	FOR_USU_ModificadoPorId INT NULL,
	FOR_FechaModificacion timestamp NULL,
	PRIMARY KEY (FOR_FormularioId),
	CONSTRAINT FK_USUARIOCREADORID_FORM FOREIGN KEY (FOR_USU_CreadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_FORM FOREIGN KEY (FOR_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_CMMESTATUSID_FORM FOREIGN KEY (FOR_CMM_EstatusId) REFERENCES ControlesMaestrosMultiples (CMM_ControlId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT 
)
//
DELIMITTER;
-- 5
DELIMITTER //
CREATE TABLE Secciones(
	SEC_CategoriaId BIGSERIAL NOT NULL,
	SEC_NombreEN VARCHAR(100) NOT NULL,
	SEC_NombreES VARCHAR(100) NOT NULL,
	SEC_FOR_FormularioId INT NOT NULL,
	SEC_SEC_PadreId INT NULL,
	SEC_CMM_EstatusId INT NOT NULL,
	SEC_USU_CreadoPorId INT NOT NULL,
	SEC_FechaCreacion timestamp NOT NULL,
	SEC_USU_ModificadoPorId INT NOT NULL,
	SEC_FechaModificacion timestamp NULL,
	PRIMARY KEY (SEC_CategoriaId),
	CONSTRAINT FK_USUARIOCREADORID_SEC FOREIGN KEY (SEC_USU_CreadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_SEC FOREIGN KEY (SEC_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_FORMULARIOID_SEC FOREIGN KEY (SEC_FOR_FormularioId) REFERENCES Formularios (FOR_FormularioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_CMMESTATUSID_SEC FOREIGN KEY (SEC_CMM_EstatusId) REFERENCES ControlesMaestrosMultiples (CMM_ControlId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT 
)
//
DELIMITTER;
-- 6
DELIMITTER //
CREATE TABLE Preguntas(
	PRE_PreguntaId BIGSERIAL NOT NULL,
	PRE_NombreEN VARCHAR(100) NOT NULL,
	PRE_NombreES VARCHAR(100) NOT NULL,
	PRE_DescripcionES VARCHAR(512) NULL,
	PRE_DescripcionEN VARCHAR(512) NULL,
	PRE_Placeholder VARCHAR(100) NULL,
	PRE_SEC_SectorId INT NOT NULL,
	PRE_CMM_TipoPreguntaId INT NOT NULL,
	PRE_Validaciones VARCHAR(1024) NULL,
	PRE_USU_CreadoPorId INT NOT NULL,
	PRE_FechaCreacion timestamp NOT NULL,
	PRE_USU_ModificadoPorId INT NULL,
	PRE_FechaModificacion timestamp NULL,
	PRIMARY KEY (PRE_PreguntaId),
	CONSTRAINT FK_USUARIOCREADORID_PRE FOREIGN KEY (PRE_USU_CreadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_PRE FOREIGN KEY (PRE_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_SECCIONID_SEC FOREIGN KEY (PRE_SEC_SectorId) REFERENCES Secciones (SEC_CategoriaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_CMMTIPOPREGUNTAD_SEC FOREIGN KEY (PRE_CMM_TipoPreguntaId) REFERENCES ControlesMaestrosMultiples (CMM_ControlId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT 
)
//
DELIMITTER;
-- 7
DELIMITTER //
CREATE TABLE Opciones(
	OPC_OpcionId BIGSERIAL NOT NULL,
	OPC_NombreEN VARCHAR(100) NOT NULL,
	OPC_NombreES VARCHAR(100) NOT NULL,
	OPC_Valor INT NULL,
	OPC_PRE_PreguntaId INT NOT NULL,
	OPC_CMM_EstatusId INT NOT NULL,
	OPC_USU_CreadoPorId INT NOT NULL,
	OPC_FechaCreacion timestamp NOT NULL,
	OPC_USU_ModificadoPorId INT NOT NULL,
	OPC_FechaModificacion timestamp NULL,
	PRIMARY KEY (OPC_OpcionId),
	CONSTRAINT FK_USUARIOCREADORID_OPC FOREIGN KEY (OPC_USU_CreadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_OPC FOREIGN KEY (OPC_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_SECCIONID_OPC FOREIGN KEY (OPC_PRE_PreguntaId) REFERENCES Preguntas (PRE_PreguntaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_CMMESTATUSID_OPC FOREIGN KEY (OPC_CMM_EstatusId) REFERENCES ControlesMaestrosMultiples (CMM_ControlId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT 
)
//
DELIMITTER;
-- 8
DELIMITTER //
CREATE TABLE Operadores(
	OPE_operadorId BIGSERIAL NOT NULL,
	OPE_USU_usuarioId INT NOT NULL,
	PRIMARY KEY (OPE_operadorId),
	CONSTRAINT FK_USUARIOCREADORID_OPE FOREIGN KEY (OPE_USU_usuarioId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 9
DELIMITTER //
CREATE TABLE Lineas(
	LIN_lineaId BIGSERIAL NOT NULL,
	LIN_USU_usuarioId INT NOT NULL,
    LIN_nombre VARCHAR(100) NOT NULL,
    LIN_codigo INT NOT NULL,
	-- CMM
    LIN_USU_CreadoPorId INT NOT NULL,
    LIN_USU_ModificadoPorId INT NULL,
    LIN_FechaCreacion timestamp NOT NULL,
    LIN_FechaModificacion timestamp NULL,
	PRIMARY KEY (LIN_lineaId),
	CONSTRAINT FK_USUARIOCREADORID_LIN FOREIGN KEY (LIN_USU_usuarioId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOCREADORID_LIN FOREIGN KEY (LIN_USU_CreadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
    CONSTRAINT FK_USUARIOCREADORID_LIN FOREIGN KEY (LIN_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 10
DELIMITTER //
CREATE TABLE Direcciones(
	DIR_DireccionId BIGSERIAL NOT NULL,
	DIR_Alias VARCHAR(250) NULL,
	DIR_PAI_PaisId INT NOT NULL,
	DIR_EST_EstadoId INT NOT NULL,
	DIR_Ciudad VARCHAR(250) NULL,
	DIR_CodigoPostal VARCHAR(10) NULL,
	DIR_Direccion VARCHAR(250) NOT NULL,
	DIR_Exterior VARCHAR(10) NULL,
	DIR_Interior VARCHAR(10) NULL,
	DIR_Latitud decimal NULL,
	DIR_Longitud decimal NULL,
	DIR_CMM_EstatusId INT NOT NULL,
	DIR_USU_CreadoPorId INT NOT NULL,
	DIR_FechaCreacion timestamp NOT NULL,
	DIR_USU_ModificadoPorId INT NULL,
	DIR_FechaModificacion timestamp NULL,
	PRIMARY KEY (DIR_DireccionId),
	CONSTRAINT FK_USUARIOCREADORID_DIR FOREIGN KEY (DIR_USU_CreadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_DIR FOREIGN KEY (DIR_USU_ModificadoPorId) REFERENCES Usuarios (USU_usuarioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_DIRECCIONESCMMESTATUSID_DIR FOREIGN KEY (DIR_CMM_EstatusId) REFERENCES ControlesMaestrosMultiples (CMM_ControlId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT 
	-- CONSTRAINT FK_DIRECCIONESCMMESTATUSID_DIR FOREIGN KEY (DIR_EST_EstadoId) REFERENCES  ()
			-- ON DELETE CASCADE
			-- ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 11
DELIMITTER //
CREATE TABLE Modulos(
	MOD_ModuloId BIGSERIAL NOT NULL,
	MOD_ModuloPadreId INT NULL,
	MOD_NombreES VARCHAR(255) NOT NULL,
	MOD_NombreEN VARCHAR(255) NOT NULL,
	MOD_Activo bit NOT NULL,
	MOD_Icono VARCHAR(255) NULL,
	MOD_Orden INT NOT NULL,
	MOD_Tipo VARCHAR(255) NOT NULL,
	MOD_URL VARCHAR(255) NULL,
	PRIMARY KEY (MOD_ModuloId)
)
//
DELIMITTER;
-- 12
DELIMITTER //
CREATE TABLE Companias(
	COM_companiaId INT NOT NULL,
	COM_nombre VARCHAR(100) NOT NULL,
	COM_razonSocial VARCHAR(100) NOT NULL,
	COM_RFC VARCHAR(50) NOT NULL,
	COM_descripcionEN VARCHAR(100) NOT NULL,
	COM_descripcionES VARCHAR(100) NOT NULL,
	-- datos rellenados para realizar pruebas
	COM_estatus VARCHAR(50) NOT NULL,
	COM_licencia VARCHAR(50) NOT NULL,
	COM_inicioContrato VARCHAR(50) NOT NULL,
	COM_correoContrato VARCHAR(50) NOT NULL,
	COM_telefono VARCHAR(50) NOT NULL,
	COM_sitioURL VARCHAR(100) NOT NULL,
	PRIMARY KEY (COM_companiaId)
)
//
DELIMITTER;
-- 13
DELIMITTER //
CREATE TABLE Almacenes(
	ALM_almacenId BIGSERIAL NOT NULL,
	ALM_COM_companiaId INT NOT NULL,
	PRIMARY KEY (ALM_almacenId),
	CONSTRAINT FK_ALAMCENCOMPANIAID_ALM FOREIGN KEY (ALM_COM_companiaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 14
DELIMITTER //
CREATE TABLE Permisos(
	PER_permisoId BIGSERIAL NOT NULL,
	PRIMARY KEY (PER_permisoId)
)
//
DELIMITTER;
-- 15
DELIMITTER //
CREATE TABLE Proveedores(
	PRV_ProveedorId BIGSERIAL NOT NULL,
	PRV_RFC VARCHAR(50) NOT NULL,
	PRV_Direccion VARCHAR(100) NOT NULL,
	PRIMARY KEY (PRV_ProveedorId)
)
//
DELIMITTER;
-- 16
DELIMITTER //
CREATE TABLE Familias(
	FAM_familiaId BIGSERIAL NOT NULL,
	FAM_COM_companiaId int NOT NULL,
	PRIMARY KEY (FAM_familiaId),
	CONSTRAINT FK_FAMILIACOMPANIAID_FAM FOREIGN KEY (FAM_COM_companiaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 17
DELIMITTER //
CREATE TABLE Categorias(
	CAT_categoriaId BIGSERIAL NOT NULL,
	CAT_FAM_familiaId INT NOT NULL,
	PRIMARY KEY (CAT_categoriaId),
	CONSTRAINT FK_CATEGORIAFAMILIAID_CAT FOREIGN KEY (CAT_FAM_familiaId) REFERENCES Familias (FAM_familiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 18
DELIMITTER //
CREATE TABLE Productos(
	PRO_productoId BIGSERIAL NOT NULL,
	PRO_CAT_categoriaId INT NOT NULL,
	PRIMARY KEY (PRO_productoId),
	CONSTRAINT FK_CATEGORIAFAMILIAID_PRO FOREIGN KEY (PRO_CAT_categoriaId) REFERENCES Categorias (CAT_categoriaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 19 
DELIMITTER //
CREATE TABLE Agrupaciones(
	AGR_agrupacionId BIGSERIAL NOT NULL,
	customer_name char(50) NOT NULL,
	PRIMARY KEY (AGR_agrupacionId)
)
//
DELIMITTER;
-- 20
DELIMITTER //
CREATE TABLE AgrupacionesProductos(
	AGP_agrupacionProductoId BIGSERIAL NOT NULL,
	AGP_AGR_agrupacionId INT NOT NULL,
	AGP_PRO_productoId INT NOT NULL,
	PRIMARY KEY (AGP_agrupacionProductoId),
	CONSTRAINT FK_AGRUPACIONESDOCAGRUPACIONESID_AGD FOREIGN KEY (AGP_AGR_agrupacionId) REFERENCES Agrupaciones (AGR_agrupacionId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_AGRUPACIONESDOCAPRODUCTOID_AGD FOREIGN KEY (AGP_PRO_productoId) REFERENCES Productos (PRO_productoId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 21
DELIMITTER //
CREATE TABLE AgrupacionesDocumentos(
	AGD_agrupacionDocumentoId BIGSERIAL NOT NULL,
	AGD_AGR_agrupacionId INT NOT NULL,
	AGD_rutaDocumento VARCHAR(1024) NOT NULL,
	PRIMARY KEY (AGD_agrupacionDocumentoId),
	CONSTRAINT FK_AGRUPACIONESDOCAGRUPACIONESID_AGD FOREIGN KEY (AGD_AGR_agrupacionId) REFERENCES Agrupaciones (AGR_agrupacionId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 22
DELIMITTER //
CREATE TABLE Movimientos(
	MOV_movimientoId BIGSERIAL NOT NULL,
	MOV_COM_companiaId INT NOT NULL,
	MOV_ALM_almacenId INT NOT NULL,
	MOV_AGR_agrupacionId INT NOT NULL,
	MOV_PRV_proveedorId INT NOT NULL,
	MOV_DIS_distribuidorId INT NOT NULL,
	MOV_CMM_tipoMovimientoId INT NOT NULL,
	MOV_CMM_origenMovimientoId INT NOT NULL,
	MOV_cantidad decimal NOT NULL,
	PRIMARY KEY (MOV_movimientoId),
	CONSTRAINT FK_MOVIMIENTOSCOMPANIAID_MOV FOREIGN KEY (MOV_COM_companiaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_MOVIMIENTOSALMACENID_MOV FOREIGN KEY (MOV_ALM_almacenId) REFERENCES Almacenes (ALM_almacenId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_MOVIMIENTOSAGRUPACIONID_MOV FOREIGN KEY (MOV_AGR_agrupacionId) REFERENCES Agrupaciones (AGR_agrupacionId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_MOVIMIENTOSPROVEDORID_MOV FOREIGN KEY (MOV_PRV_proveedorId) REFERENCES Proveedores (PRV_proveedorId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
	-- CONSTRAINT FK_MOVIMIENTOSDISTRIBUIDORID_MOV FOREIGN KEY (MOV_DIS_distribuidorId) REFERENCES  ()
			-- ON DELETE CASCADE
			-- ON UPDATE RESTRICT,
	-- CONSTRAINT FK_MOVIMIENTOSTIPOMOVIMIENTOID_MOV FOREIGN KEY (MOV_CMM_tipoMovimientoId) ControlesMaestrosMultiples REFERENCES  ()
			-- ON DELETE CASCADE
			-- ON UPDATE RESTRICT,
	-- CONSTRAINT FK_MOVIMIENTOSORIGENMOVIMIENTOID_MOV FOREIGN KEY (MOV_CMM_origenMovimientoId) ControlesMaestrosMultiples REFERENCES  ()
			-- ON DELETE CASCADE
			-- ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 23 
DELIMITTER //
CREATE TABLE ProveedoresCompanias(
	PCO_ProveedorCompaniaId BIGSERIAL NOT NULL,
	PCO_PRV_ProveedorId INT  NOT NULL,
	PCO_COM_CompaniaId INT NOT NULL,
	PCO_Alias VARCHAR(500) NULL,
	PRIMARY KEY (PCO_ProveedorCompaniaId),
	CONSTRAINT FK_PROVEDORCOMPANIAPROVEDORID_PCO FOREIGN KEY (PCO_PRV_ProveedorId) REFERENCES Proveedores (PRV_ProveedorId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_PROVEDORCOMPANIACOMPANIAID_PCO FOREIGN KEY (PCO_COM_CompaniaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 24
DELIMITTER //
CREATE TABLE Operaciones(
	OPR_operacionId BIGSERIAL NOT NULL,
	OPR_COM_companiaId INT NOT NULL,
	OPR_OPE_operadorId INT NOT NULL,
	OPR_LIN_lineaId INT NOT NULL,
	OPR_AGR_agrupacionId INT NOT NULL,
	OPR_EMB_embalajeId INT NOT NULL,
	OPR_CMM_tipoOperacionId INT NOT NULL,
	OPR_codigoInicial VARCHAR(20) NOT NULL,
	OPR_codigoFinal VARCHAR(20) NULL,
	OPR_cantidad decimal NOT NULL,
	PRIMARY KEY (OPR_operacionId),
	CONSTRAINT FK_OPERACIONCOMPANIAID_OPR FOREIGN KEY (OPR_COM_companiaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	-- CONSTRAINT FK_ALAMCENCOMPANIAID_OPR FOREIGN KEY (OPR_OPE_operadorId) REFERENCES  ()
			-- ON DELETE CASCADE
			-- ON UPDATE RESTRICT,
	CONSTRAINT FK_OPERADORLINEAID_OPR FOREIGN KEY (OPR_LIN_lineaId) REFERENCES Lineas (LIN_lineaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_OPERADORAGRUPACIONID_OPR FOREIGN KEY (OPR_AGR_agrupacionId) REFERENCES Agrupaciones (AGR_agrupacionId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	-- CONSTRAINT FK_OPERADOREMBALAJEID_OPR FOREIGN KEY (OPR_EMB_embalajeId) REFERENCES  ()
			-- ON DELETE CASCADE
			-- ON UPDATE RESTRICT,
	CONSTRAINT FK_OPERADORCMMID_OPR FOREIGN KEY (OPR_CMM_tipoOperacionId) REFERENCES ControlesMaestrosMultiples (CMM_ControlId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 25
DELIMITTER //
CREATE TABLE RolesPermisos(
	RPE_rolPermisoId SERIAL NOT NULL,
	RPE_ROL_rolId INT NOT NULL,
	RPE_PER_permisoId INT NOT NULL,
	RPE_MOD_moduloId INT NOT NULL,
	PRIMARY KEY (RPE_rolPermisoId),
	CONSTRAINT FK_ROLPERMISOROLID_RPE FOREIGN KEY (RPE_ROL_rolId) REFERENCES Roles (ROL_rolId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_ROLPERMISOPERMISOID_RPE FOREIGN KEY (RPE_PER_permisoId) REFERENCES Permisos (PER_permisoId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_ROLPERMISOMODULOID_RPE FOREIGN KEY (RPE_MOD_moduloId) REFERENCES Modulos (MOD_ModuloId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 26 
DELIMITTER //
CREATE TABLE UsuariosCompanias(
	USC_usuarioCompaniaId BIGSERIAL NOT NULL,
	USC_USU_usuarioId INT NOT NULL,
	USC_COM_companiaId INT NOT NULL,
	PRIMARY KEY (USC_usuarioCompaniaId),
	CONSTRAINT FK_USUARIOID_USC FOREIGN KEY (USC_USU_usuarioId) REFERENCES Usuarios (USU_usuarioId)
				ON DELETE CASCADE
				ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOCOMPANIAID_USC FOREIGN KEY (USC_COM_companiaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 27
DELIMITTER //
CREATE TABLE UsuariosRolesCompañias(
	URC_usuarioRolCompaniaId BIGSERIAL NOT NULL,
	URC_USC_usuarioCompaniaId INT NOT NULL,
	URC_RPE_rolPermisoId INT NOT NULL,
	PRIMARY KEY (URC_usuarioRolCompaniaId),
	CONSTRAINT FK_USUARIOROLCOMPANIAID_URC FOREIGN KEY (URC_USC_usuarioCompaniaId) REFERENCES UsuariosCompanias (USC_usuarioCompaniaId)
				ON DELETE CASCADE
				ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOROLCOMPANIAPERMISOID_URC FOREIGN KEY (URC_RPE_rolPermisoId) REFERENCES RolesPermisos (RPE_rolPermisoId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 28
DELIMITTER //
CREATE TABLE ProveedoresFormularios(
    PFR_ProveedorFormularioId BIGSERIAL NOT NULL,
    PFR_FOR_FormularioId INT NOT NULL,
    PFR_PRV_ProveedorId INT NOT NULL,
    PRIMARY KEY (PFR_ProveedorFormularioId),
    CONSTRAINT FK_PROVEDORFORMULARIOID_PFR FOREIGN KEY (PFR_FOR_FormularioId) REFERENCES Formularios (FOR_FormularioId)
            ON DELETE CASCADE
            ON UPDATE RESTRICT,
    CONSTRAINT FK_PROVEDORPROVEEDORESID_PFR FOREIGN KEY (PFR_PRV_ProveedorId) REFERENCES Proveedores (PRV_ProveedorId)
            ON DELETE CASCADE
            ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 29
DELIMITTER //
CREATE TABLE FormulariosCompanias(
	FRC_FormularioCompaniaId BIGSERIAL NOT NULL,
	FRC_FOR_FormularioId INT NOT NULL,
	FRC_COM_CompaniaId INT NOT NULL,
	PRIMARY KEY (FRC_FormularioCompaniaId),
	CONSTRAINT FK_COMPANIAID_FRC FOREIGN KEY (FRC_COM_CompaniaId) REFERENCES Companias (COM_companiaId )
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_FORMULARIOID_FRC FOREIGN KEY (FRC_FOR_FormularioId) REFERENCES Formularios (FOR_FormularioId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 30
DELIMITER //
CREATE TABLE Autonumericos(
	AUT_AutonumericoId BIGSERIAL NOT NULL,
	AUT_COM_CompaniaId INT NOT NULL,
	AUT_Nombre VARCHAR(50) NOT NULL,
	AUT_Prefijo VARCHAR(5) NOT NULL,
	AUT_Siguiente INT NOT NULL,
	AUT_Digitos INT NOT NULL,
	AUT_Activo bit NOT NULL,
	PRIMARY KEY (AUT_AutonumericoId),
	CONSTRAINT FK_AUTONUMERICOCOMPANIAID_CM FOREIGN KEY (AUT_COM_CompaniaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;
-- 31
DELIMITER //
CREATE TABLE ControlesMaestros(
	CM_ControlMaestroId BIGSERIAL NOT NULL,
	CM_COM_CompaniaId INT NOT NULL,
	CM_Nombre VARCHAR(50) NOT NULL,
	CM_VALORES VARCHAR (255)NOT NULL,
	CM_VALOREN VARCHAR (255)NOT NULL,
	CM_Sistema bit NOT NULL,
	CM_FechaModificacion timestamp NULL,
	PRIMARY KEY (CM_ControlMaestroId),
	CONSTRAINT FK_CONTROLMAESTROCOMPANIAID_CM FOREIGN KEY (CM_COM_CompaniaId) REFERENCES Companias (COM_companiaId)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
//
DELIMITTER;

<<<<<<< HEAD
-- 32
DELIMITER //
CREATE TABLE SociosDeNegocios(
	soc_sociodenegocioid BIGSERIAL NOT NULL,
	soc_nombre VARCHAR(100) NOT NULL,
	soc_razonsocial VARCHAR(100) NOT NULL,
	soc_rfc VARCHAR(50) UNIQUE NOT NULL ,
	soc_usu_creadoporid INT NOT NULL,
	soc_fechacreacion timestamp NOT NULL,
	soc_usu_modificadoporid INT NULL,
	soc_fechamodificacion timestamp NULL,
	soc_cmm_estatusid INT NOT NULL,
	PRIMARY KEY (soc_sociodenegocioid)
	
)
//
DELIMITTER;


-- 33

CREATE TABLE SociosDeNegociosCompania(
	snc_sociodenegociocompaniaid BIGSERIAL NOT NULL,
	snc_com_companiaid INT NOT NULL,
	snc_cmm_estatusid INT NOT NULL,
	snc_nombre VARCHAR(100) NOT NULL,
	snc_usu_creadoporid INT NOT NULL,
	snc_fechacreacion TIMESTAMP NOT NULL,
	snc_usu_modificadoporid INT NULL,
	snc_fechamodificacion TIMESTAMP NULL,
	PRIMARY KEY (SNC_socioDeNegocioIdCompania),
	CONSTRAINT FK_USUARIOCREADORID_SNC FOREIGN KEY (snc_usu_creadoporid) REFERENCES Usuarios (usu_usuarioid)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_SNC FOREIGN KEY (snc_usu_modificadoporid) REFERENCES Usuarios (usu_usuarioid)
			ON DELETE CASCADE
			ON UPDATE RESTRICT.
	CONSTRAINT
)
=======
CREATE TABLE SociosDeNegocios(
	soc_sociodenegocioid BIGSERIAL NOT NULL,
	soc_nombre VARCHAR(50) NOT NULL,
	soc_razonsocial VARCHAR (50)NOT NULL,
	soc_rfc VARCHAR (50)NOT NULL,
	soc_usu_creadoporid INT NOT NULL,
	soc_usu_modificadoporid INT NOT NULL,
	PRIMARY KEY (soc_sociodenegocioid)
)

private Integer id;

    @Column(name="soc_nombre")
    private String nombre;

    @Column(name="soc_razonsocial")
    private String razonsocial;

    @Column(name="soc_rfc")
    private String rfc;

   @Column(name="soc_usu_creadoporid")
    private Integer creadoporid;

   // @Column(name="soc_fechacreacion")
   // private String fechacreacion;

   @Column(name="soc_usu_modificadoporid")
   private Integer modificadoporid;

    //@Column(name="soc_fechamodificacion")
   // private String fechamodificacion;
>>>>>>> b1f9ffb77541710cbefd0afd2f5cf5b196f5667d
