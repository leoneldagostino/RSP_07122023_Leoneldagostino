using Entidades.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Excepciones
{
    public class ComidaInvalidaExeption : Exception
    {
        /// <summary>
        /// La excepcion sera lanzada cuando al buscar el tipo de comida no sea existente, esta excepcion sera guardada en un archivo con los datos y mensaje establecidos
        /// </summary>
        /// <param name="message">El mensaje que sera establecido para informar el error</param>
        public ComidaInvalidaExeption(string? message) : base(message)
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
    }
}
