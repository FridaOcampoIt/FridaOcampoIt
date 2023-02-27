	CREATE TABLE direcciones(
	dir_direcionid BIGSERIAL NOT NULL,
	dir_soc_sociodenegocioid INT NULL,
	dir_com_companiaid INT NULL,
	dir_usu_usuarioid INT NULL,
	dir_paisid INT NOT NULL,
	dir_estadoid INT NOT NULL,
	dir_ciudadprovincia VARCHAR(500) NULL,
	dir_direccion VARCHAR(500) NOT NULL,
	dir_numeroexterior VARCHAR(10) NOT NULL,
	dir_numerointerior VARCHAR(10) NULL,
	dir_latitud DECIMAL NULL,
	dir_logitud DECIMAL NULL,
	dir_cmm_estatus INT NOT NULL,
	dir_usu_creadoporid INT,
	dir_fechacreacion TIMESTAMP NOT NULL,
	dir_usu_modificadoporid INT NULL,
	dir_fechamodificacion TIMESTAMP NULL,
	PRIMARY KEY(dir_direcionid)
)
insert into direcciones (
	dir_direcionid ,
	dir_soc_sociodenegocioid ,
	dir_com_companiaid ,
	dir_usu_usuarioid ,
	dir_paisid ,
	dir_estadoid ,
	dir_ciudadprovincia ,
	dir_direccion ,
	dir_numeroexterior ,
	dir_numerointerior ,
	dir_latitud ,
	dir_logitud ,
	dir_cmm_estatus ,
	dir_usu_creadoporid ,
	dir_fechacreacion ,
	dir_usu_modificadoporid ,
	dir_fechamodificacion 
	
)values(
	1,
	1,
	1,
	1,
	2,
	3,
	'chiapas',
	'juan junipero cerra',
	156,
	null,
	17.1545,
	5.26584,
	1000001,
	1,
	now(),
	1,
	now()

)
-- CREACION DE TABLA COMPANIA PRA PRUEBAS
CREATE TABLE companias(
	com_companiaid SERIAL NOT NULL,
    com_nombre VARCHAR(100) NOT NULL,
    com_razonsocial VARCHAR(100) NOT NULL,
    com_rfc VARCHAR(50) NOT NULL,
    com_descripcion VARCHAR(100) NOT NULL,
    com_iniciocontrato TIMESTAMP NOT NULL,
    com_numerocliente INT,
    com_correocontacto VARCHAR(50) NOT NULL,
    com_telefono VARCHAR(50) NOT NULL,
    com_sitiourl VARCHAR(100) NOT NULL,
    com_usu_creadoporid INT NOT NULL,
    com_fechacreacion TIMESTAMP NOT NULL,
    com_usu_modificadoporid INT,
    com_fechamodificacion TIMESTAMP,
    com_cmm_estatus INT NOT NULL,
    com_cmm_licencia INT NOT NULL,
	PRIMARY KEY (com_companiaid)
)
INSERT INTO companias (
	com_companiaid ,
    com_nombre ,
    com_razonsocial ,
    com_rfc ,
    com_descripcion ,
    com_iniciocontrato ,
    com_numerocliente ,
    com_correocontacto ,
    com_telefono ,
    com_sitiourl ,
    com_usu_creadoporid ,
    com_fechacreacion ,
    com_usu_modificadoporid ,
    com_fechamodificacion ,
    com_cmm_estatus ,
    com_cmm_licencia 
)VALUES(
	1,
	'compania',
	'razon social',
	'rfcderogfg',
	'descripcion aaaaa',
	now(),
	1,
	'licencia@gmail.com',
	'3335022186',
	'sitio www.weajskd.com',
	1,
	now(),
	1,
	now(),
	1000001,
	3
	)

CREATE TABLE cedis(
	ced_cediid BIGSERIAL NOT NULL,
	ced_nombre VARCHAR(50) NOT NULL,
	ced_numeroserie VARCHAR (50)NOT NULL,
	ced_usu_modificadoporid INT,
	ced_usu_creadoporid INT NOT NULL,
	ced_com_companiaid INT,
	ced_cmm_estatusid integer NOT NULL,
	ced_telefono INT,
	ced_fechacreacion TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    ced_fechamodificacion TIMESTAMP WITHOUT TIME ZONE,
    ced_dir_direcionid INT,
	PRIMARY KEY (ced_cediid),
	CONSTRAINT FK_USUARIOCREADORID_CED FOREIGN KEY (ced_usu_creadoporid) REFERENCES Usuarios (usu_usuarioid)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_USUARIOMODIFICADOID_CED FOREIGN KEY (ced_usu_modificadoporid) REFERENCES Usuarios (usu_usuarioid)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_COMPANIAID_CED FOREIGN KEY (ced_com_companiaid) REFERENCES companias (com_companiaid)
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_ESTATUSID_CED FOREIGN KEY (ced_cmm_estatusid) REFERENCES controlesmaestrosmultiples (cmm_controlid )
			ON DELETE CASCADE
			ON UPDATE RESTRICT,
	CONSTRAINT FK_DIRECCIONID_CED FOREIGN KEY (ced_dir_direcionid) REFERENCES direcciones (dir_direcionid)
			ON DELETE CASCADE
			ON UPDATE RESTRICT
)
insert into cedis (
	ced_cediid ,
	ced_nombre ,
	ced_numeroserie ,
	ced_usu_modificadoporid ,
	ced_usu_creadoporid ,
	ced_com_companiaid ,
	ced_cmm_estatusid ,
	ced_telefono ,
	ced_fechacreacion ,
    ced_fechamodificacion,
	ced_dir_direcionid)
	values
	(
		1,
		'prueba cedis',
		'A4V5D8REVDFGDV',
		1,
		1,
		0,
		1000001,
		33350,
		now(),
		now(),
		1	
	)