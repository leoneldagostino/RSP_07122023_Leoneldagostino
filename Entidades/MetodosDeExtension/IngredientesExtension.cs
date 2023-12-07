using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        /// <summary>
        /// El metodo realizara un calculo del costo de la comida recorriendo los ingredientes y tomara sus valores establecidos
        /// </summary>
        /// <param name="ingredientes">La lista de los ingredientes de la comida la cual va a recorrer</param>
        /// <param name="costoInicial">El monto inicial que tiene la comida</param>
        /// <returns>Devolvera el monto final de la comida </returns>
        public static double CalcularCostoIngrediente(this List<EIngrediente> ingredientes, int costoInicial)
        {
            double costoFInal = costoInicial;

            foreach (EIngrediente ingrediente in ingredientes)
            {
                double costoAgregado = (int)ingrediente;
                costoFInal += costoInicial * costoAgregado / 100;
            }
            return costoFInal;
        }
        /// <summary>
        /// El metodo de extension de rand realizara la eleccion de los ingredientes de forma aleatoria de una lista de ingredientes
        /// </summary>
        /// <param name="rand">parametro que sera un random</param>
        /// <returns>Devolvera la lista de ingredientes aleatorios tomando elementos random de la lista</returns>
        public static List<EIngrediente> IngredienteAleatorios(this Random rand)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.ADHERESO,
                EIngrediente.QUESO,
                EIngrediente.JAMON,
                EIngrediente.HUEVO,
                EIngrediente.PANCETA
            };

            int numeroRand = rand.Next(1, ingredientes.Count + 1);
     
            return ingredientes.Take(numeroRand).ToList();
        }
    }
}




