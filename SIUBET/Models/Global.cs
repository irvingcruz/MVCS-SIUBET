using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIUBET.Models
{
    public static class Global
    {
        public static string vMsgSuccess = "El registró se guardó correctamente.";
        public static string vMsgFail = "Error al tratar de guadar el registro.";
        public static string vMsgThrow = "Error inesperado al procesar el registro.";
        public static string vMsgUserFail = "No existe un usuario autenticado";
        public static string vMsgFileSizeFail = "Tamaño máximo del archivo 500 KB. (Archivo: ";
        public static string vMsgFileTypefail = "Solo esta permitido documentos PDF.";
        public static int iMaxSizeFile = 512000; //500KB - 0.5MB
        public static string vPDF = "pdf";
        public static string vUsuario = "";
    }
}