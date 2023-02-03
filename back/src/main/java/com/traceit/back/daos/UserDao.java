package com.traceit.back.daos;

import com.traceit.back.models.User;
import com.traceit.back.projections.User.UserListProjection;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.repository.query.Param;

import java.util.List;

public interface UserDao extends CrudRepository<User, String> {

    List<User> findAllBy();
    User findUsuarioByNombre(String nombre);
    User findByIdIn(List<Integer> ids);

    //Projection queries
    List<UserListProjection> findAllProjectedBy();
}
