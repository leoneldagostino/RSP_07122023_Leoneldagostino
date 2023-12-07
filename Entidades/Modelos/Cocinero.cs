using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using System.Runtime.CompilerServices;

namespace Entidades.Modelos
{
    public delegate void DelegadoPedidoEnCurso(IComestible menu);
    public delegate void DelegadoDemoraAtencion(double demora);


    public class Cocinero<T> 
        where T : IComestible, new()

    {
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoPedidoEnCurso OnPedido;

        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T pedidosEnPreparacion;
        private Task tarea;

        //CAMBIOS NUEVOS
        private Mozo<T> mozo;
        private Queue<T> queue;





        /// <summary>
        /// Constructor de la clase cocinero
        /// </summary>
        /// <param name="nombre">Establecera el nombre del cocinero</param>
        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.queue = new Queue<T>();

            this.mozo.OnPedido += TomarNuevoPedido;
        }

        private void TomarNuevoPedido(T menu)
        {
            if(OnPedido != null)
            {
                queue.Enqueue(menu);
            }
        }


        //No hacer nada
        public Queue<T> Pedidos
        {
            get { return queue; }
        } 
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
                    this.mozo.EmpezarATrabajar = true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.mozo.EmpezarATrabajar = false;
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
        private void EmpezarACocinar()
        {
            
            this.tarea = Task.Run((Action)(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    if(queue.Count > 0)
                    {
                        
                        pedidosEnPreparacion = queue.Dequeue();
                        EsperarProximoIngreso();
                        this.cantPedidosFinalizados++;
                        try
                        {
                            DataBaseManager.GuardarTicket<T>(Nombre, (T)this.pedidosEnPreparacion);
                        
                        }
                        catch(DataBaseManagerException ex)
                        {
                            FileManager.Guardar(ex.Message, "logs.txt", true);
                     
                        }
                    }

                }
            }));
        }
        /// <summary>
        /// El metodo chequeara si no existe un ingreso, inicializara la preparacion y despues invocara el evento
        /// </summary>



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
                while(!this.cancellation.IsCancellationRequested && pedidosEnPreparacion.Estado)
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
