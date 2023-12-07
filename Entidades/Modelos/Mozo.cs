using Entidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoPedido<T>(T menu);
    public class Mozo <T> where T : IComestible, new()
    {
        public event DelegadoNuevoPedido <T> OnPedido;



        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;

        public bool EmpezarATrabajar 
        {
            get
            {
                if(tarea != null && (tarea.Status == TaskStatus.Running || tarea.Status == TaskStatus.WaitingToRun || tarea.Status == TaskStatus.WaitingForActivation))
                {
                    return true;
                }
                return false;

            }
            set
            {
                if(value == true && (tarea == null || tarea.Status != TaskStatus.Running || tarea.Status != TaskStatus.WaitingToRun || tarea.Status != TaskStatus.WaitingForActivation))
                {
                    cancellation = new CancellationTokenSource();
                    TomarPedidos();
                }
                else
                {
                    cancellation.Cancel();
                }
            }
        }



        private void NotificarNuevoPedido()
        {
            if (this.OnPedido != null)
            {
                this.menu = new T();

                this.menu.InicializarPreparacion();

                this.OnPedido.Invoke(this.menu);
            }

        }
        private void TomarPedidos()
        {
            if(!cancellation.IsCancellationRequested) 
            { 
                Thread.Sleep(5000);
                tarea = Task.Run(() =>
                {
                    NotificarNuevoPedido();
                },cancellation.Token);
            }

        }


    }
}
