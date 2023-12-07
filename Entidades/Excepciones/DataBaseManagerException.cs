using Entidades.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class DataBaseManagerException : Exception
    {
        /// <summary>
        /// La excepcion se lanzara en caso de que ocurra un error cuando se maneje la base datos, guardandolo en un archivo aquellos errores que ocurra
        /// </summary>
        /// <param name="message">El mensaje que sera establecido para informar el error</param>
        public DataBaseManagerException(string? message) : base(message)
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

        public DataBaseManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
