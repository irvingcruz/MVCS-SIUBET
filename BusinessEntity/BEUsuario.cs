using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class BEUsuario
    {
        public int IDUsuario { get; set; }
        [Required(ErrorMessage = "Favor de ingresar un Usuario Válido.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Favor de ingresar el Password.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Favor de ingresar su Nombre Completo.")]
        public string Nombres { get; set; }
        public int IDPerfil { get; set; }
        public string Grupo { get; set; }
        public bool Recordarme { get; set; }
    }
}
