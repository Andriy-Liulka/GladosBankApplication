using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GladosBank.Api.Config.Athentication
{
    public class JwtAuthenticationOptions
    {
        public static string Issuer = "GladosBank.Api"; 
        public static string Audience = "GladosBank.Api";
        private static RsaSecurityKey RsaKey;
        //GladosBank148820011212
        private readonly string _privateKey = "MIIEogIBAAKCAQBlHwcC/cJgj8QJvdJkRm7rz680I9Q1uuhIeeoY+el1Kq7jNjgFqhhsBISi5589ZLHTafisFgq8F28UljBOZB6++doN7hqqrq+mkF6jdze1iVVOwfIrlnJnigYBvmIPwbrGCspJM+dYvW2OgL8xiFM4zTt5CoZcZ8szAQh0Y/srWgYn+qwuLO1a2qppJRJwfdRQiu0dSVgE+8IoacwswWUcVeYJCTcNQF6H5k2Zvoo7/fiz4Hj62EhA5xKjigeE7nnd2F+Rho3gWC7+Z+0VIrLuXDe7cMh068/IFZ8OpWD4iwZJ62s1c/AshzmBH1pT8lT4BRyUSgdVkVct2qvYj1ttAgMBAAECggEAWFrO9pchMtwLL3PTlFhSbOqvrIAt2LCyazlTCXW7veuImfDy7EgeRKuB9REq+yqzkgVDCaHMyWI0ZamBFc23a6jGFWvZkw9FXOVCO9RcMduIVWNzJRC0b7GS5A0kg54Dgy1EaMfYOZpyUpQ9+pqiFLyGXZGLqsvqKWuappABk3NeDkl17OQLeGK5ZfV3TJCV4qgIT0qZWP/9wI6CwTgphg1yEakTbXWgSmpIDo2bM3mPQeBWENeD2dPCbIJojVThEWFJiYLpVLPtTLEEmv+KYBIwUfJ2xCVUruRmCUI/JwXTRXK8eleQM3DerLcRmyUppq5yAv/e674IQmCvpY9LIQKBgQC7MZnhD52rMgsufQgDYpWppH/zmRwikwV3Y+eMqL1rayQMkzNxAsYoNIMTzm2TRbBA4kYR3uyOR+39edQ3CNrix7er/c5WOOivLpYyJLaFx9631wNzkRVEbsMGV1XP4wHVwaMqUv4JIzd/RY+CGSroVQKiD5CH0Wfhr4rYKJUddQKBgQCKSj6HoS6GxI5wE57tuKzoKEkjdmcwicnzBCas507VDfyU6qRq8zZPMzE3K7MPSUrbqX6mSEaHuvxBc7Qw+GgOUPIi1VLbLnAFtmOZohMbX1zZWlXt5eYJJWoEsi1jHJqgybR5PsBfDv5Fye3nlvIo5Po5BtHRi2i0XJOwxVsvGQKBgQC0mToYhHRdQj6/bylX+rjhni9D2afnQ2i2stAmSHXXONritvGeSkrbZy4+4Z2dxFIzhxqLC0O2UXcuYWd4YTasLHrrmEaFsQnsWUnWvszJbPdW7j4KNSMLjsDhUUkc86pFjDfbEY69Shi01WuK3LUkyT9tEw8huco7VpzhxuV2MQKBgCd2CiPPtaQVQIPwxWAWW5ifrkclSUrLdsoWvCaIlrErHZEs55/xKOUuuXOBz8Wj522JDy/J3f5rOAJBTwDOUXsMVBvmfY830BWBNyim3AbqjmcjZPPOr3aaq4cNhPAMQH9zL0GNy30UnTAy7+Uu6IkM1e4h6Qt/WXBmHmrJ1jw5AoGAGIgYKldP6fQUaEVnARWDLWVNVV3wFiLLMN8fI/ykMmp//upzLAPWTmiBt8STLhOEiXU/RSaCXnRmeI7wi+xyFPT9wyjNqS057Bhe3Oc+xce9jAgfAHjRrIE7Ry/SxPPAt9BVoopA9j8H00pJWbUa+sg1WMwpYWU8so0YsuTqQr8=";

        public JwtAuthenticationOptions()
        {
            RSA rsa = RSA.Create();
            var privateKeyBytes = Convert.FromBase64String(_privateKey);
            rsa.ImportRSAPrivateKey(privateKeyBytes, out int _);
            RsaKey = new RsaSecurityKey(rsa);
        }


        public static RsaSecurityKey GetSecurityKey()
        {

            var jwt = new JwtAuthenticationOptions();
            var privateKey = RsaKey;
            return privateKey;
        }
    }
}
