﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class BEUsuario
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Recordarme { get; set; }
        public string Nombres { get; set; }
        public int IDPerfil { get; set; }
    }
}