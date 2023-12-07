using Entidades.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class FileManagerException : Exception
    {
        /// <summary>
        /// La excepcion se lanzara cuando ocurra un error al manejar archivos. Guardara la excepcion, la hora, y el mensaje.
        /// </summary>
        /// <param name="message">Sera el mensaje de error por el cual lanza la excepcion</param>
        public FileManagerException(string? message) : base(message)
        {
            string nombreArchivo = "Logs.txt";
            StringBuilder datosError = new StringBuilder();
            datosError.AppendLine("--------Excepcion--------");
            datosError.AppendLine($"Hora: {DateTime.Now}");
            datosError.AppendLine($"Tipo de excepcion: {this.GetType()}");
            datosError.AppendLine($"Error: {message}");

            string datosExpecion = datosError.ToString();

            FileManager.Guardar(datosExpecion, nombreArchivo, true);
        }
        public FileManagerException(string message, Exception? innerException) : base(message, innerException) 
        {
           
        }

    }
}
