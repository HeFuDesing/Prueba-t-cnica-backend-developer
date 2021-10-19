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
    public class LoginController : ControllerBase
    {
        test_Hector_FunesContext db = new test_Hector_FunesContext();
        [HttpPost]
        public RespuestaLogin Post([FromBody] Login Registro)
        {
            string sSecretKey = Request.Headers.FirstOrDefault(x => x.Key == "SecretKey").Value;
            if (sSecretKey != "123456")
            {
                Response.StatusCode = (Int32)HttpStatusCode.Unauthorized;
                return new RespuestaLogin();
            }
            if (string.IsNullOrEmpty(Registro.email))
            {
                Response.StatusCode = (Int32)HttpStatusCode.BadRequest;
                return new RespuestaLogin();
            }
            if (string.IsNullOrEmpty(Registro.password))
            {
                Response.StatusCode = (Int32)HttpStatusCode.BadRequest;
                return new RespuestaLogin();
            }
            if (db.Users.Where(u => u.Email == Registro.email && u.Password == Registro.password).Count() == 0)
            {
                if (db.Users.Where(u => u.Email == Registro.email).Count() == 1)
                {
                    User modelo = db.Users.Where(u => u.Email == Registro.email).FirstOrDefault();
                    return new RespuestaLogin()
                    {
                        last_login = modelo.LastLogin,
                        name = modelo.Name
                    };
                }
            }
            User Usuario = db.Users.Where(u => u.Email == Registro.email && u.Password == Registro.password).FirstOrDefault();
            DateTime? ultimoLogin = Usuario.LastLogin;
            Usuario.LastLogin = DateTime.Now;
            db.Entry(Usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();

            DateTime Vencimiento = DateTime.Now.AddMonths(1);
            BasicToken obj = new BasicToken();
            obj.Vencimiento = Vencimiento.ToString("dd/MM/yyyy hh:mm:ss");
            obj.Usuario = Usuario.Name;
            obj.Key = Guid.NewGuid().ToString();
            string sEncriptado = obj.Serialize();

            return new RespuestaLogin()
            {
                token = new Token()
                {
                    token = sEncriptado, 
                    expiration = Vencimiento
                }, 
                last_login = ultimoLogin,
                name = Usuario.Name
            };
        }
    }
}
