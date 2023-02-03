package com.traceit.back.controllers;

import com.traceit.back.daos.UserDao;
import com.traceit.back.models.JsonResponse;
import com.traceit.back.models.User;
import com.traceit.back.projections.User.UserListProjection;
import com.traceit.back.utils.JWT;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping(value = "/api/v1/users")
public class UsersController {
    @Autowired
    private UserDao userDao;

    @RequestMapping(value = "/list", method = RequestMethod.GET)
    public JsonResponse getAllUsers(){
        List<UserListProjection> usersList = userDao.findAllProjectedBy();
        //List<User> usersList = userDao.findAllBy();
        return new JsonResponse(usersList, "All users are here", JsonResponse.STATUS_OK) ;
    }

    @RequestMapping(value = "/create", method = RequestMethod.POST)
    public JsonResponse createUserRobert(HttpServletRequest req){
        User robert = new User();
        robert.setNombre("Robert");
        robert.setContrasenia("patito123");

        userDao.save(robert);
        return new JsonResponse(null, "Robert is now owner of a half of teddy city!", JsonResponse.STATUS_OK);
    }
}
