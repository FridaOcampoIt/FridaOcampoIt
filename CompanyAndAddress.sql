
CREATE TABLE IF NOT EXISTS public.companias
(
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
    CONSTRAINT companias_pkey PRIMARY KEY (com_companiaid),
    CONSTRAINT companias_com_rfc_key UNIQUE (com_rfc),
    CONSTRAINT fk_com_cmm_estatus FOREIGN KEY (com_cmm_estatus)
        REFERENCES controlesmaestrosmultiples (cmm_controlid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_com_cmm_licencia FOREIGN KEY (com_cmm_licencia)
        REFERENCES controlesmaestrosmultiples (cmm_controlid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_com_usu_creadoporid  FOREIGN KEY (com_usu_creadoporid)
        REFERENCES usuarios (usu_usuarioid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_com_usu_modificadoporid FOREIGN KEY (com_usu_modificadoporid)
        REFERENCES usuarios (usu_usuarioid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);


CREATE TABLE IF NOT EXISTS public.direcciones
(
    dir_direcionid BIGSERIAL NOT NULL,
    dir_soc_sociodenegocioid INT ,
    dir_com_companiaid INT ,
    dir_usu_usuarioid INT ,
    dir_paisid INT  NOT NULL,
    dir_estadoid INT  NOT NULL,
    dir_ciudadprovincia VARCHAR(100),
    dir_direccion VARCHAR(100) NOT NULL,
    dir_numeroexterior VARCHAR(100) NOT NULL,
    dir_numerointerior VARCHAR(100),
    dir_latitud DECIMAL,
    dir_logitud DECIMAL,
    dir_cmm_estatus INT  NOT NULL,
    dir_usu_creadoporid INT,
    dir_fechacreacion TIMESTAMP NOT NULL,
    dir_usu_modificadoporid INT,
    dir_fechamodificacion TIMESTAMP,
    CONSTRAINT direcciones_pkey PRIMARY KEY (dir_direcionid),
    CONSTRAINT fk_dir_com_companiaid FOREIGN KEY (dir_com_companiaid)
        REFERENCES companias (com_companiaid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_dir_cmm_estatus FOREIGN KEY (dir_cmm_estatus)
        REFERENCES controlesmaestrosmultiples (cmm_controlid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_dir_usu_creadoporid FOREIGN KEY (dir_usu_creadoporid)
        REFERENCES usuarios (usu_usuarioid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_dir_usu_modificadoporid FOREIGN KEY (dir_usu_modificadoporid)
        REFERENCES usuarios (usu_usuarioid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);


CREATE TABLE SociosDeNegocios(
	soc_sociodenegocioid BIGSERIAL NOT NULL,
	soc_nombre VARCHAR(50) NOT NULL,
	soc_razonsocial VARCHAR (50)NOT NULL,
	soc_rfc VARCHAR (50) UNIQUE NOT NULL,
	soc_usu_creadoporid INT NOT NULL,
	soc_usu_modificadoporid INT NOT NULL,
	soc_fechacreacion TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    soc_fechamodificacion TIMESTAMP WITHOUT TIME ZONE,
	soc_cmm_estatusid integer NOT NULL,
	PRIMARY KEY (soc_sociodenegocioid)
);




/* Tipo Licencia */ 
insert into controlesmaestrosmultiples
	(cmm_controlid, cmm_control, cmm_valores, cmm_valoren, cmm_activo, cmm_sistema, cmm_usu_creadoporid, cmm_usu_modificadoporid, cmm_fechacreacion, cmm_fechamodificacion)
values
	(1000020, 'CMM_TIPO_LICENCIA', 'Reproceso', 'Rework', true, true, null, null, now(), null),
	(1000021, 'CMM_TIPO_LICENCIA', 'N3', 'N3', true, true, null, null, now(), null),
	(1000022, 'CMM_TIPO_LICENCIA', 'N4', 'N4', true, true, null, null, now(), null);

INSERT INTO companias (

     com_companiaid,
    com_nombre,
    com_razonsocial,
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
    com_cmm_licencia ,
)
VALUES(
     1,
    'compania',
    'razonsocial',
    'oefr991206',
    'descripcn',
    now(),
    1,
    'contrato@hotmail',
    '3335022186',
    'www.gmail.com',
    1,
    now(),
    1,
    now(),
    1000000,
    1000000,
)