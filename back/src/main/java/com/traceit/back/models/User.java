package com.traceit.back.models;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;
import org.hibernate.id.factory.spi.GenerationTypeStrategy;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import java.util.Collection;
import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Entity
@Table(name = "usuarios")
public class User implements UserDetails {


    @Id
    @SequenceGenerator(name="usuarios_usu_usuarioid_seq", sequenceName="usuarios_usu_usuarioid_seq", allocationSize=1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator="usuarios_usu_usuarioid_seq")
    @Column(name = "usu_usuarioId", columnDefinition = "bigserial", insertable = false, updatable = false)
    private Integer id;

    @Column(name = "usu_nombre")
    private String nombre;

    @Column(name = "usu_contrasenia")
    private String contrasenia;


    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        return List.of(new SimpleGrantedAuthority("USER"));
    }

    @Override
    public String getPassword() { return this.contrasenia; }

    @Override
    public String getUsername() { return this.nombre; }

    @Override
    public boolean isAccountNonExpired() { return true; }

    @Override
    public boolean isAccountNonLocked() { return true; }

    @Override
    public boolean isCredentialsNonExpired() { return true; }

    @Override
    public boolean isEnabled() { return true; }
}
