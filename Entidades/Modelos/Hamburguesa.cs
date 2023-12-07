using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {

        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;
        static Hamburguesa() => Hamburguesa.costoBase = 1500;

        public string Imagen
        {
            get => imagen; 
        }
        public bool Estado
        {
            get => estado;
        }
      


        public Hamburguesa() : this(false) { }
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }

        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";
        /// <summary>
        /// El metodo verifica si el estado de la clase, establece su estado en verdadero y un numero randmon entre 1 y 9
        /// traera la imagen mediante el metodo de GetImagenComida
        /// llamara al metodo de agregar ingredientes
        /// </summary>
        public void InicializarPreparacion()
        {
            if (!Estado)
            {
                int numeroRand = new Random().Next(1, 9);

                this.imagen =DataBaseManager.GetImageComida($"Hamburguesa_{numeroRand}");

                AgregarIngredientes();
                this.estado = true;
            }
        }
        /// <summary>
        /// El metodo establecera los ingredientes mediante el metodo de extension "IngredienteAleteatorios".
        /// </summary>
        private void AgregarIngredientes()
        {
            ingredientes = IngredientesExtension.IngredienteAleatorios(new Random());

        }

        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();

        }


        public override string ToString() => this.MostrarDatos();

        /// <summary>
        /// El metodo finalizara la preparacion de la comida estableciendo el costo con el metodo de extension "calcularCostoIngrediente" 
        /// Cambiara el estado de la hamburguesa
        /// </summary>
        /// <param name="cocinero">Recibira el cocinero el cual finaliza la preparacion</param>
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = IngredientesExtension.CalcularCostoIngrediente(this.ingredientes, costoBase);

            this.estado = false;
            
        }
    }
}