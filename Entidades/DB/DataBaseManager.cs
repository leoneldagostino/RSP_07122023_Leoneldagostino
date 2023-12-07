using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static string stringConnection;
        static SqlConnection connection;
        /// <summary>
        /// Se establece la conexion a la base de datos
        /// </summary>
        static DataBaseManager()
        {
            stringConnection = "Server=.;Database=tpFInal;Trusted_Connection=True;";
        }
        /// <summary>
        /// El metodo accedera a al base de datos y obtendra la url de la imagen que se encuentra en ella.
        /// </summary>
        /// <param name="tipo">sera el tipo de comida que buscara en la base de datos</param>
        /// <returns>Devolvera la url de la imagen</returns>
        /// <exception cref="ComidaInvalidaExeption">Lanzara la excepcio en caso de que el tipo de comida que se busca no se encuentre en la base de datos</exception>
        /// <exception cref="Exception">Lanzara la excepcion en caso de que ocurra un error</exception>
        public static string GetImageComida(string tipo)
        {
            string query = "SELECT * FROM comidas WHERE tipo_comida = @tipo";
            string imagen = string.Empty;
            try
            {
                using (DataBaseManager.connection = new SqlConnection(stringConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(query, DataBaseManager.connection))
                    {
                        cmd.Parameters.AddWithValue("@tipo", tipo);
                        DataBaseManager.connection.Open();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            
                            if (reader.Read())
                            {
                                imagen = reader.GetString(2);
                            }
                        
                            else
                            {
                                throw new ComidaInvalidaExeption("No existe el tipo de comida");
                            }
                        }
                    }
                
                }
            }
            catch(ComidaInvalidaExeption ex)
            {
                throw;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception error)
            {
                throw new Exception($"Error al acceder a la base de datos: {error}");
            }
            return imagen;  

        }

        /// <summary>
        /// El metodo genera un ticket guardandolo en la base de datos
        /// </summary>
        /// <typeparam name="T">El tipo del cual obtendra datos del tipo</typeparam>
        /// <param name="nombreEmpleado">Sera el nombre del empleado que genera el ticket</param>
        /// <param name="comida">Sera el dato que posteriormente se obtendra su datos para ingresar en la base de datos</param>
        /// <returns>Devolvera  verdadero una vez realizado el ticket</returns>
        /// <exception cref="DataBaseManagerException">Lanzara la excepcion en caso de que ocurra error cuando agregue el ticket a la base de datos </exception>
        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            string query = "INSERT INTO tickets (empleado, ticket) VALUES (@nombreEmpleado, @comida)";

            try
            {
                using (connection = new SqlConnection(stringConnection))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreEmpleado", nombreEmpleado);
                        command.Parameters.AddWithValue("@comida", comida.Ticket);
                        connection.Open();
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DataBaseManagerException("Ocurrio un error al insertar en la base de datos",ex);
            }
            catch(Exception ex)
            {
                throw new DataBaseManagerException("Ocurrio un error ", ex);
            }
            return true;

        }
    }
}
