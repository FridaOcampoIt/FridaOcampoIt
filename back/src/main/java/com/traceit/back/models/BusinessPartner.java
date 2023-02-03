package com.traceit.back.models;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;

import java.util.Date;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Entity
@Table(name = "SociosDeNegocios")
public class BusinessPartner {

    @Id
    @SequenceGenerator(name="SociosDeNegocios_soc_sociodenegocioid_seq", sequenceName="SociosDeNegocios_soc_sociodenegocioid_seq", allocationSize=1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator="SociosDeNegocios_soc_sociodenegocioid_seq")
    @Column(name = "soc_sociodenegocioid", columnDefinition = "bigserial", insertable = false, updatable = false)
    private Integer id;

    @Column(name="soc_nombre")
    private String nombre;

    @Column(name="soc_razonsocial")
    private String razonsocial;

    @Column(name="soc_rfc")
    private String rfc;

   @Column(name="soc_usu_creadoporid")
    private Integer creadoporid;

  // @Column(name="soc_fechacreacion")
   // private Date fechacreacion;

   @Column(name="soc_usu_modificadoporid")
   private Integer modificadoporid;
   // @Column(name="soc_fechamodificacion")
    //private Date fechamodificacion;

    @Column(name="soc_cmm_controlId")
    private Integer controlId;

}
