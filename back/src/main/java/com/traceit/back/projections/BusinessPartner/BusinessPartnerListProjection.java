package com.traceit.back.projections.BusinessPartner;

import com.traceit.back.models.BusinessPartner;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;
import org.springframework.data.rest.core.config.Projection;

import java.util.Date;

@Projection(types= {BusinessPartner.class})
public interface BusinessPartnerListProjection {
    //Aqui se hace un filtro de lso datos que se quieran consultar
    Integer getid();
    String getnombre();
    String getrazonsocial();
    String getrfc();
    Integer getcreadoporid();
    Integer getmodificadoporid();
    @CreationTimestamp
    Date getfechacreacion();
    @UpdateTimestamp
    Date getfechamodificacion();
   Integer getestatusid();


}
