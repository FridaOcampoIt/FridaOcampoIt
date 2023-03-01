CREATE TABLE IF NOT EXISTS public.cedis(
	ced_cediid BIGSERIAL NOT NULL,
	ced_nombre VARCHAR(50) NOT NULL,
	ced_numeroserie INT NULL,
	ced_usu_modificadoporid INT,
	ced_usu_creadoporid INT NOT NULL,
	ced_com_companiaid INT,
	ced_cmm_estatusid integer NOT NULL,
	ced_telefono VARCHAR(10)NULL,
	ced_fechacreacion TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    ced_fechamodificacion TIMESTAMP WITHOUT TIME ZONE,
    ced_dir_direcionid INT,
	PRIMARY KEY (ced_cediid),
	CONSTRAINT FK_USUARIOCREADORID_CED FOREIGN KEY (ced_usu_creadoporid) REFERENCES Usuarios (usu_usuarioid)
			 ON UPDATE NO ACTION
        ON DELETE NO ACTION,
	CONSTRAINT FK_USUARIOMODIFICADOID_CED FOREIGN KEY (ced_usu_modificadoporid) REFERENCES Usuarios (usu_usuarioid)
			 ON UPDATE NO ACTION
        ON DELETE NO ACTION,
	CONSTRAINT FK_COMPANIAID_CED FOREIGN KEY (ced_com_companiaid) REFERENCES companias (com_companiaid)
			 ON UPDATE NO ACTION
        ON DELETE NO ACTION,
	CONSTRAINT FK_ESTATUSID_CED FOREIGN KEY (ced_cmm_estatusid) REFERENCES controlesmaestrosmultiples (cmm_controlid )
			 ON UPDATE NO ACTION
        ON DELETE NO ACTION,
	CONSTRAINT FK_DIRECCIONID_CED FOREIGN KEY (ced_dir_direcionid) REFERENCES direcciones (dir_direcionid)
			 ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    UNIQUE(ced_numeroserie,ced_com_companiaid)
)
-- ESTADO DE CEDIS
insert into controlesmaestrosmultiples
	(cmm_controlid, cmm_control, cmm_valores, cmm_valoren, cmm_activo, cmm_sistema, cmm_usu_creadoporid, cmm_usu_modificadoporid, cmm_fechacreacion, cmm_fechamodificacion)
values
	(1000030, 'CMM_ESTATUS_CEDIS', 'Activo', 'Active', true, true, null, null, now(), null),
	(1000031, 'CMM_ESTATUS_CEDIS', 'Inactivo','Inactive', true, true, null, null, now(), null),
	(1000032, 'CMM_ESTATUS_CEDIS', 'Borrado', 'Deleted', true, true, null, null, now(), null);
-- cedis 
INSERT INTO modulos 
 (mod_nodoid, mod_nodopadre, mod_nombre, mod_icono, mod_tipo, mod_orden, mod_url, mod_sistema)
VALUES
 (10700, 10000, 'cedis', null, 'basic', 1, '/backoffice/cedis', true)

-- ROL cedis
 insert into rolesmodulos
  (rlm_rol_rolid, rlm_mod_nodoid, rlm_crear, rlm_modificar, rlm_eliminar, rlm_usu_creadoporid, rlm_fechacreacion)
 values
  (1, 10700, true, true, true, null, now())


--- PRUEBA INSERCION COMPANIA
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
    com_cmm_licencia 
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
    1000000
)