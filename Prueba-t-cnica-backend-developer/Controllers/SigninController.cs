using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_t_cnica_backend_developer.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Prueba_t_cnica_backend_developer.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SigninController : ControllerBase
    {
        test_Hector_FunesContext db = new test_Hector_FunesContext();
        [HttpPost]
        public Respuesta Post([FromBody]Register Registro)
        {
            string sSecretKey = Request.Headers.FirstOrDefault(x => x.Key == "SecretKey").Value;
            if (sSecretKey != "123456")
            {
                Response.StatusCode = (Int32)HttpStatusCode.Unauthorized;
                return new Respuesta()
                {
                    Exito = false,
                    Mensaje = "Desaturoizado"
                };
            }
            if (string.IsNullOrEmpty(Registro.name))
            {
                return new Respuesta()
                {
                    Exito = false,
                    Mensaje = "Favor enviar el nombre del usuario"
                };
            }
            if (string.IsNullOrEmpty(Registro.email))
            {
                return new Respuesta()
                {
                    Exito = false,
                    Mensaje = "Favor enviar el email del usuario"
                };
            }
            if (string.IsNullOrEmpty(Registro.password))
            {
                return new Respuesta()
                {
                    Exito = false,
                    Mensaje = "Favor enviar el password del usuario"
                };
            }
            if (db.Users.Where(u => u.Email == Registro.email).Count() > 0)
            {
                return new Respuesta()
                {
                    Exito = false,
                    Mensaje = "Correo ya registrado en base de datos"
                };
            }
            db.Users.Add(new Modelos.User()
            {
                Email = Registro.email,
                Name = Registro.name,
                Password = Registro.password
            });
            db.SaveChanges();
            return new Respuesta()
            {
                Exito = true,
                Mensaje = "Usuario Registrado con Exito"
            };
        }
    }
}
