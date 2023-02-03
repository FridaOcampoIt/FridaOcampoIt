package com.traceit.back.controllers;

import com.traceit.back.daos.BusinessPartnerDao;
import com.traceit.back.models.BusinessPartner;
import com.traceit.back.models.JsonResponse;
import com.traceit.back.projections.BusinessPartner.BusinessPartnerListProjection;
import jakarta.servlet.http.HttpServletRequest;
import lombok.extern.java.Log;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.repository.query.Param;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping (value= "/api/v1/BusinessPartner")
public class BusinessPartnerController {
    @Autowired
    private BusinessPartnerDao BusinessPartnerDao;

    @RequestMapping(value="/list", method = RequestMethod.GET)
    public JsonResponse getAllBusinessPartner(){
        List<BusinessPartnerListProjection> BusinessPartnerList = BusinessPartnerDao.findAllProjectedBy();

        return  new JsonResponse(BusinessPartnerList, "Todos los socios de negocio",JsonResponse.STATUS_OK);
    }
    //entcontrar solo uno
    @RequestMapping(value="/list/{id}", method = RequestMethod.GET)
    public JsonResponse getBusinessPartnerId(@PathVariable Integer id) {
        List<BusinessPartnerListProjection> BusinessPartnerList = BusinessPartnerDao.findAllProjectedBy();

        for (BusinessPartnerListProjection a : BusinessPartnerList){
            System.out.println("Texto del mensaje "+a.getid());
            if (a.getid()==id){
                return new JsonResponse(a, "socio de negocio", JsonResponse.STATUS_OK);
            }
        }
        return null;

    }


    @RequestMapping(value="/create", method = RequestMethod.POST)
    public JsonResponse createBusinessPartner(HttpServletRequest req){
        BusinessPartner BusinessPartner= new BusinessPartner();

        BusinessPartner.setNombre("provedor2");
        BusinessPartner.setRazonsocial("LALA");
       BusinessPartner.setRfc("ertsdfcxzswe");
        BusinessPartner.setCreadoporid(3);
        BusinessPartner.setModificadoporid(3);
      //  BusinessPartner.setFechacreacion("02/12/2323");
       //  BusinessPartner.setFechamodificacion("02/12/2323");

        BusinessPartnerDao.save(BusinessPartner);
        return new JsonResponse( null,"Socio de negocio creado satisfactoriamente",JsonResponse.STATUS_OK);
    }
        //https://inezpre5.wordpress.com/2018/05/01/tutorial-crud-spring-boot-aplicacion-con-bases-de-datos/
    @RequestMapping(value="/update/{id}", method = RequestMethod.PUT)
    public JsonResponse updateBusinessPartner(HttpServletRequest req){
        List<BusinessPartnerListProjection> BusinessPartnerList = BusinessPartnerDao.findAllProjectedBy();
        BusinessPartner BusinessPartner= new BusinessPartner();

        BusinessPartner.setNombre("provedor2");
        BusinessPartner.setRazonsocial("pascal");
        BusinessPartner.setRfc("99");
        BusinessPartner.setCreadoporid(1);
        BusinessPartner.setModificadoporid(1);
        //BusinessPartner.setFechacreacion("02/12/4000");
        //BusinessPartner.setFechamodificacion("02/12/4000");

        BusinessPartnerDao.save(BusinessPartner);
        return new JsonResponse( null,"Socio de negocio a sido modificaco",JsonResponse.STATUS_OK);
    }

    @RequestMapping(value="/delete", method = RequestMethod.PUT)
    public JsonResponse deleteBusinessPartner(HttpServletRequest req){
        BusinessPartner BusinessPartner= new BusinessPartner();

        BusinessPartner.setNombre("provedor2");
        BusinessPartner.setRazonsocial("pascal");
        BusinessPartner.setRfc("99");
       // BusinessPartner.setUsu_creadoporid(1);
       // BusinessPartner.setUsu_modificadoporid(1);
        //BusinessPartner.setFechacreacion("02/12/4000");
        //BusinessPartner.setFechamodificacion("02/12/4000");

        BusinessPartnerDao.save(BusinessPartner);
        return new JsonResponse( null,"Socio de negocio a sido eliminado",JsonResponse.STATUS_OK);
    }
}
