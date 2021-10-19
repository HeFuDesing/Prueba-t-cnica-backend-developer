using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_t_cnica_backend_developer.Modelos
{
    public class RespuestaLogin
    {
        public Token token { get; set; }
        public DateTime? last_login { get; set; }
        public string name { get; set; }
    }
    public class Token
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
