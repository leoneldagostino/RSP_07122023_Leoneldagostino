using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    
    public static class FileManager
    {
        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        /// <summary>
        /// Se intancia la ruta correspondiente y su correspondiente validacion que exista
        /// </summary>
        static FileManager()
        {
            FileManager.path += "\\20231207_DagostinoLeonel\\";
            FileManager.ValidaExistenciaDirectorio();
        }
        /// <summary>
        /// Se valida que el directorio que se encuentra en el atributo path exista, caso contrario la creara.
        /// </summary>
        /// <exception cref="FileManagerException">En caso de que exista un problema para crear el directorio.</exception>
        private static void ValidaExistenciaDirectorio()
        {
            
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);

                }
                catch 
                { 
                    throw new FileManagerException("Error al crear directorio");
                        
                }
            }
        }
        /// <summary>
        /// El motodo creara un archivo y lo guardara en una ruta ya establecida
        /// </summary>
        /// <param name="data">Es la informacion que se guardara en el archivo</param>
        /// <param name="NombreArchivo">Es el nombre que tendra el archivo</param>
        /// <param name="append">es el parametro que indicara si se sobreescribe o se agrega a la data existente</param>
        /// <exception cref="FileManagerException">Es la excepcion que lanzara en caso de que ocurra un error al guardar el archivo</exception>        
        public static void Guardar(string data, string NombreArchivo,bool append)
        {
            try
            {
                string rutaCompleta = Path.Combine(path, NombreArchivo);
                using(StreamWriter sw = new StreamWriter(rutaCompleta,append))
                {
                    sw.WriteLine(data);

                }
            }
            catch
            {
                throw new FileManagerException("Ocurrio un error al guardar el archivo");
            }
        }


        /// <summary>
        /// El metodo serealizara en json y guardara el archivo serealizado en una ruta ya establecida
        /// </summary>
        /// <typeparam name="T">El tipo de elemento que sera serealizado</typeparam>
        /// <param name="elemento">El elemento que sera serealizado</param>
        /// <param name="nombreArchivo">El nombre del archivo una vez ya serealizado</param>
        /// <returns>Devolvera true una vez que el proceso se haya realizado</returns>
        /// <exception cref="FileManagerException">La excepecion se lanzara en caso de que ocurra un erro al serializar el archivo</exception>
        public static bool Serealizar<T>(T elemento, string nombreArchivo) where T : class 
        {
            try
            {
                JsonSerializerOptions configuracion = new JsonSerializerOptions();
                configuracion.WriteIndented = true;
                string json = JsonSerializer.Serialize(elemento,configuracion);
                Guardar(json, nombreArchivo, false);
              
            }
            catch
            {
                throw new FileManagerException("Ocurrio un error al serializar el archivo");
            }
            return true;
        }
    }
}
                    
                    
              
     