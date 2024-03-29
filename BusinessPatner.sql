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
	PRIMARY KEY (soc_sociodenegocioid),
	CONSTRAINT FK_ESTATUSID_SOC FOREIGN KEY (soc_cmm_estatusid) REFERENCES controlesmaestrosmultiples (cmm_controlid )
			 ON UPDATE NO ACTION
        ON DELETE NO ACTION
);


-- socio de negocios
INSERT INTO modulos 
 (mod_nodoid, mod_nodopadre, mod_nombre, mod_icono, mod_tipo, mod_orden, mod_url, mod_sistema)
VALUES
 (10800, 10000, 'sociosdenegocios', null, 'basic', 1, '/backoffice/sociosdenegocios', true);

-- ROL socios de negocios
 insert into rolesmodulos
  (rlm_rol_rolid, rlm_mod_nodoid, rlm_crear, rlm_modificar, rlm_eliminar, rlm_usu_creadoporid, rlm_fechacreacion)
 values
  (1, 10800, true, true, true, null, now());

-- socios de negocios
insert into controlesmaestrosmultiples
	(cmm_controlid, cmm_control, cmm_valores, cmm_valoren, cmm_activo, cmm_sistema, cmm_usu_creadoporid, cmm_usu_modificadoporid, cmm_fechacreacion, cmm_fechamodificacion)
values
	(1000040, 'CMM_ESTATUS_BusinesPatner', 'Activo', 'Active', true, true, null, null, now(), null),
	(1000041, 'CMM_ESTATUS_BusinesPatner', 'Inactivo','Inactive', true, true, null, null, now(), null),
	(1000042, 'CMM_ESTATUS_BusinesPatner', 'Borrado', 'Deleted', true, true, null, null, now(), null);

	