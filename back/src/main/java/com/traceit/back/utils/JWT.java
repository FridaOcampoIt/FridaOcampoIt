package com.traceit.back.utils;

import com.traceit.back.models.User;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import io.jsonwebtoken.security.Keys;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.security.core.userdetails.UserDetails;

import java.nio.charset.StandardCharsets;
import java.security.Key;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.function.Function;

public class JWT {

    private static String SIGNING_KEY = "7x!A%D*G-KaPdSgUkXp2s5v8y/B?E(H+MbQeThWmZq3t6w9z$C&F)J@NcRfUjXn2";

    public static String generateToken(String issuer, User user){
        return generateToken(issuer, "u/"+user.getId(), user, new HashMap<>());
    }

    public static String generateToken(String issuer, User user, Map<String, Object> claims){
        return generateToken(issuer, "u/"+user.getId(), user, claims);
    }

    public static String generateToken(String issuer, String subject, User user, Map<String, Object> claims) {
        return Jwts.builder()
                .setIssuer(issuer)
                .setSubject(subject)
                .setClaims(claims).setSubject(user.getUsername())
                .setIssuedAt(new Date(System.currentTimeMillis()))
                //.setExpiration(new Date(System.currentTimeMillis() + 100 * 60 * 24))
                .signWith(getSigninKey(), SignatureAlgorithm.HS256)
                .compact();
    }

    public static Integer getUsuarioId(HttpServletRequest req){
        try{
            String token = ((HttpServletRequest) req).getHeader("Autorization").substring(7);
            Claims claims = Jwts.parserBuilder().setSigningKey(getSigninKey()).build().parseClaimsJws(token).getBody();
            return (Integer) claims.get("id");
        } catch (Exception e){
            return -1;
        }
    }

    public static String extractUsername(String token){
        return extractClaim(token, Claims::getSubject);
    }

    private static Date extractExpiration(String token){
        Date expiration = extractClaim(token, Claims::getExpiration);
        return expiration == null ? new Date() : expiration;
    }

    private static Claims extractAllClaims(String token){
        return Jwts.parserBuilder()
                .setSigningKey(getSigninKey()).build().parseClaimsJws(token).getBody();
    }

    public static <T> T extractClaim(String token, Function<Claims, T> claimsResolver) {
        final Claims claims = extractAllClaims(token);
        return claimsResolver.apply(claims);
    }

    private static Key getSigninKey(){
        byte[] key = SIGNING_KEY.getBytes(StandardCharsets.UTF_8);
        return Keys.hmacShaKeyFor(key);
    }

    public static boolean isTokenValid(String token, UserDetails userDetails) {
        final String username = extractUsername(token);
        return (username.equals(userDetails.getUsername()) && !isTokenExpired(token));
    }

    private static boolean isTokenExpired(String token){
        return extractExpiration(token).before(new Date());
    }
}
