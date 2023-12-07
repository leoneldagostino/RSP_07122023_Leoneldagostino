using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange
            string texto = "Este es una pruba";
            //bool excepcionLanzada = false;

            //act
            FileManager.Guardar(texto, "no/_mbre Invalido", false);
            
            //assert
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero;
            int cantidadPedidosEsperados = 0;
            //act
            cocinero = new Cocinero<Hamburguesa>("Cocinero de prueba");
            //assert

            Assert.AreEqual(cantidadPedidosEsperados, cocinero.CantPedidosFinalizados);
        }
    }
}