using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using System.Runtime.CompilerServices;

namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoIngreso(IComestible menu);
    public delegate void DelegadoDemoraAtencion(double demora);


    public class Cocinero<T> 
        where T : IComestible, new()

    {
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoNuevoIngreso OnIngreso;

        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;




        /// <summary>
        /// Constructor de la clase cocinero
        /// </summary>
        /// <param name="nombre">Establecera el nombre del cocinero</param>
        public Cocinero(string nombre)
        {
            this.nombre = nombre;
        }

        //No hacer nada
        
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.IniciarIngreso();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        /// <summary>
        /// El metodo realizara realizara en otro hilo el ingreso, de nuevos clientes.
        /// </summary>
        private void IniciarIngreso()
        {
            CancellationToken token = this.cancellation.Token;
            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    NotificarNuevoIngreso();
                    EsperarProximoIngreso();
                    this.cantPedidosFinalizados++;
                    try
                    {
                        DataBaseManager.GuardarTicket<T>(Nombre, menu);
                        
                    }
                    catch(DataBaseManagerException ex)
                    {
                        FileManager.Guardar(ex.Message, "logs.txt", true);
                     
                    }

                }
            },token);
        }
        /// <summary>
        /// El metodo chequeara si no existe un ingreso, inicializara la preparacion y despues invocara el evento
        /// </summary>
        private void NotificarNuevoIngreso()
        {
            if (this.OnIngreso != null)
            {
                this.menu = new T();

                this.menu.InicializarPreparacion();

                this.OnIngreso.Invoke(this.menu);
            }
        }


        /// <summary>
        /// El metodo chequeara si no existe el evento de demora y mientras no se cancele y el estado de la preparacion este activa, pausara durante 1 segundo el hilo, aumentando en tiempo de espera en 1.
        /// Invocando el evento OnDemora
        /// Agrega el tiempo total a la demora de la preparacion total
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;
            if (this.OnDemora != null)
            {
                while(!this.cancellation.IsCancellationRequested && menu.Estado)
                {
                    Thread.Sleep(1000);

                    tiempoEspera++;
                    this.OnDemora.Invoke(tiempoEspera);
                }

            }

            this.demoraPreparacionTotal += tiempoEspera;

        }
    }
}
