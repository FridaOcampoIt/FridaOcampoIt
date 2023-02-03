package com.traceit.back.controllers;

import com.traceit.back.daos.BusinessPartnerDao;
import com.traceit.back.models.BusinessPartner;
import com.traceit.back.models.JsonResponse;
import com.traceit.back.projections.BusinessPartner.BusinessPartnerListProjection;
import jakarta.servlet.http.HttpServletRequest;
import lombok.extern.java.Log;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.repository.query.Param;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping (value= "/api/v1/BusinessPartner")
public class BusinessPartnerController {
    @Autowired
    private BusinessPartnerDao BP;

    @RequestMapping(value="/list", method = RequestMethod.GET)
    public JsonResponse getAllBusinessPartner(){
        List<BusinessPartnerListProjection> BusinessPartnerList = BP.findAllProjectedBy();

        return  new JsonResponse(BusinessPartnerList, "Todos los socios de negocio",JsonResponse.STATUS_OK);
    }
    //entcontrar solo uno
    @RequestMapping(value="/list/{id}", method = RequestMethod.GET)
    public JsonResponse getBusinessPartnerId(@PathVariable("id") String id) {
        Optional<BusinessPartner> BusinessPartnerList = BP.findById(id);

        return new JsonResponse(BusinessPartnerList, "socio de negocio", JsonResponse.STATUS_OK);

    }


    @RequestMapping(value="/create", method = RequestMethod.POST)
    public JsonResponse createBusinessPartner(HttpServletRequest req){
        BusinessPartner BusinessPartner= new BusinessPartner();

        BusinessPartner.setNombre("numero 1");
        BusinessPartner.setRazonsocial("razon 1");
        BusinessPartner.setRfc("987456513");
        BusinessPartner.setCreadoporid(1);
        BusinessPartner.setModificadoporid(1);
        BusinessPartner.setControlId(1000000);
       // BusinessPartner.setFechacreacion();
        //BusinessPartner.setFechamodificacion();

        BP.save(BusinessPartner);
        return new JsonResponse( null,"Socio de negocio creado satisfactoriamente",JsonResponse.STATUS_OK);
    }
        //https://inezpre5.wordpress.com/2018/05/01/tutorial-crud-spring-boot-aplicacion-con-bases-de-datos/

    @RequestMapping(value="/update/{id}", method = RequestMethod.PUT)
    public JsonResponse updateBusinessPartner(HttpServletRequest req, BusinessPartner businessPartner ){

        businessPartner.setRazonsocial("otro nombre");
        businessPartner.setRfc("RFC NUEVOOO");
        businessPartner.setNombre("LENOVOOOO");
        businessPartner.setCreadoporid(55);
        businessPartner.setModificadoporid(55);

        BP.save(businessPartner);

        return new JsonResponse(businessPartner, "Socio de negocio a sido modificaco", JsonResponse.STATUS_OK);
    }


}
