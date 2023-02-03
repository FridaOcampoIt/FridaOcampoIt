package com.traceit.back.daos;

import com.traceit.back.models.BusinessPartner;
import com.traceit.back.models.User;
import com.traceit.back.projections.BusinessPartner.BusinessPartnerListProjection;
import org.springframework.data.repository.CrudRepository;

import java.util.List;

public interface BusinessPartnerDao extends CrudRepository<BusinessPartner,String> {

    List<BusinessPartner> findAllBy();

    BusinessPartner findBusinessPartnerByNombre(String nombre);
    BusinessPartner findBusinessPartnerByrazonsocial(String razonsocial);
    BusinessPartner findBusinessPartnerByrfc(String rfc);
    BusinessPartner findBusinessPartnerBycreadoporid(Integer creadoporid);
    BusinessPartner findBusinessPartnerBymodificadoporid(Integer modificadoporid);
    BusinessPartner findByIdIn (List<Integer> ids);

    List<BusinessPartnerListProjection> findAllProjectedBy();
}
