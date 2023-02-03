package com.traceit.back.controllers;

import com.traceit.back.daos.UserDao;
import com.traceit.back.models.JsonResponse;
import com.traceit.back.models.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.env.Environment;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import java.util.HashMap;
import java.util.Map;

import com.traceit.back.utils.JWT;

@RestController
@RequestMapping(value = "/api/v1/auth")
public class AuthController {
    @Autowired
    private Environment env;
    @Autowired
    private PasswordEncoder passwordEncoder;
    @Autowired
    private UserDao userDao;

    @RequestMapping(value = "/login", method = RequestMethod.POST)
    public JsonResponse login(@RequestBody Map<String, String> credentials) {
        String name = credentials.get("username");
        String pass = credentials.get("password");
        if (name == null || pass == null)
            //TODO: Change 4 custom exception and feedback message
            return new JsonResponse(null, null, JsonResponse.STATUS_ERROR);
        User user = userDao.findUsuarioByNombre(name);
        if(user == null)
            //TODO: Change 4 custom exception and feedback user not found
            return new JsonResponse(null, null, JsonResponse.STATUS_ERROR);
        if(!passwordEncoder.matches(pass, user.getPassword()))
            //TODO: Change 4 custom exception and feedback password missmatch
            return new JsonResponse(null, null, JsonResponse.STATUS_ERROR);

        HashMap<String,Object> claims = new HashMap<>();
        claims.put("rol", "SUPER_ADMIN");
        claims.put("permisos", "Todos los permisos");
        String issuer = env.getProperty("environments.custom.company");
        String token = JWT.generateToken(issuer, user, claims);

        HashMap<String, Object> json = new HashMap<>();
        json.put("token", token);

        return new JsonResponse(json, null, JsonResponse.STATUS_OK);
    }
}
