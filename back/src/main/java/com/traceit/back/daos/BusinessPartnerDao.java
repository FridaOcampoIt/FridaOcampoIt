package com.traceit.back.daos;

import com.traceit.back.models.BusinessPartner;
import com.traceit.back.models.User;
import com.traceit.back.projections.BusinessPartner.BusinessPartnerListProjection;
import org.springframework.data.repository.CrudRepository;

import java.util.List;

public interface BusinessPartnerDao extends CrudRepository<BusinessPartner,String> {

    BusinessPartner findByIdIn (List<Integer> ids);
    BusinessPartner deleteById(Integer id);

    List<BusinessPartnerListProjection> findAllProjectedBy();
}
