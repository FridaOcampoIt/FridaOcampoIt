package com.traceit.back.projections.BusinessPartner;

import com.traceit.back.models.BusinessPartner;
import org.springframework.data.rest.core.config.Projection;

import java.sql.Date;

@Projection(types= {BusinessPartner.class})
public interface BusinessPartnerListProjection {
    //Aqui se hace un filtro de lso datos que se quieran consultar
    Integer getid();
    String getnombre();
    String getrazonsocial();
    String getrfc();
    Integer getcreadoporid();
    Integer getmodificadoporid();

}
