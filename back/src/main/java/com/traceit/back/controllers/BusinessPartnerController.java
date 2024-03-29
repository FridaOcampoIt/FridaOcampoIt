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

import java.util.Date;
import java.util.List;
import java.util.Optional;

@RestController
@CrossOrigin(origins = "http://localhost:4200")
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
    public JsonResponse createBusinessPartner(HttpServletRequest req,@RequestBody BusinessPartner businessPartner){
        businessPartner.setFechacreacion(new Date());
        businessPartner.setFechamodificacion(new Date());
        businessPartner.setCreadoporid(12);
        businessPartner.setModificadoporid(12);
        businessPartner.setEstatusid(1000000);
        businessPartner= BP.save(businessPartner);

        return new JsonResponse( businessPartner.getId(),"Socio de negocio creado satisfactoriamente",JsonResponse.STATUS_OK);
    }
        //https://inezpre5.wordpress.com/2018/05/01/tutorial-crud-spring-boot-aplicacion-con-bases-de-datos/

    @RequestMapping(value="/update/{id}", method = RequestMethod.PUT)
    public JsonResponse updateBusinessPartner(HttpServletRequest req,@RequestBody BusinessPartner businessPartner ){


        businessPartner.setModificadoporid(56);
        businessPartner.setEstatusid(1000000);
        businessPartner.setFechamodificacion(new Date());

        businessPartner= BP.save(businessPartner);

        return new JsonResponse(businessPartner, "Socio de negocio a sido modificaco", JsonResponse.STATUS_OK);
    }

    @RequestMapping(value="/delete/{id}", method = RequestMethod.DELETE)
    public JsonResponse deleteBusinessPartnerId(@PathVariable("id") String id) {
        BP.deleteById(id);
        return new JsonResponse(null, "socio de negocio elimiado", JsonResponse.STATUS_OK);
    }




}
