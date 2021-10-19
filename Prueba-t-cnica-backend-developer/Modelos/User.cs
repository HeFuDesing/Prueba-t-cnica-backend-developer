using System;
using System.Collections.Generic;

#nullable disable

namespace Prueba_t_cnica_backend_developer.Modelos
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
