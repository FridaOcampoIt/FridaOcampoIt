package com.traceit.back.projections.User;

import com.traceit.back.models.User;
import org.springframework.data.rest.core.config.Projection;

@Projection(types = {User.class})
public interface UserListProjection {
    Integer getId();
    String getNombre();
}
