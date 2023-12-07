using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.Modelos;


namespace FrmView
{
    public partial class FrmView : Form
    {
        
        Cocinero<Hamburguesa> hamburguesero;
        private IComestible comida;


        /// <summary>
        /// El constructor inicializara los componentes, establecera los atributos y susbcribira al los eventos
        /// </summary>
        public FrmView()
        {
            InitializeComponent();
            this.hamburguesero = new Cocinero<Hamburguesa>("Ramon");
            //Alumno - agregar manejadores al cocinero

            this.hamburguesero.OnDemora += this.MostrarConteo;
            this.hamburguesero.OnPedido += this.MostrarComida;
        }


        //Alumno: Realizar los cambios necesarios sobre MostrarComida de manera que se refleje
        //en el formulario los datos de la comida
        /// <summary>
        /// El evento mostrar comida mostrara por pantalla la imagen, sus ingredientes y lo agregara a la cola de la lista de comida
        /// En caso de que no 
        /// </summary>
        /// <param name="comida"></param>
        private void MostrarComida(IComestible comida)
        {
            if(this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarComida(comida));
            }
            else
            {
                
                this.comida = comida;
                this.pcbComida.Load(comida.Imagen);
                this.rchElaborando.Text = comida.ToString();
            }
        }


        //Alumno: Realizar los cambios necesarios sobre MostrarConteo de manera que se refleje
        //en el fomrulario el tiempo transucurrido
        /// <summary>
        /// El evento mostrar conteo lo que realizara sera invocarse el en caso de que sea requerido, caso contrario mostrara por el tiempo en pantalla la actualizacion de la misma.
        /// </summary>
        /// <param name="tiempo"></param>
        private void MostrarConteo(double tiempo)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(() => MostrarConteo(tiempo));
            }
            else
            {
                this.lblTiempo.Text = $"{tiempo} segundos";
                this.lblTmp.Text = $"{this.hamburguesero.TiempoMedioDePreparacion.ToString("00.0")} segundos";
            }
        }



        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (!this.hamburguesero.HabilitarCocina)
            {
                this.hamburguesero.HabilitarCocina = true;
                this.btnAbrir.Image = Properties.Resources.close_icon;
            }
            else
            {
                this.hamburguesero.HabilitarCocina = false;
                this.btnAbrir.Image = Properties.Resources.open_icon;
            }

        }
        /// <summary>
        /// El evento lo que realizara sera la evaluacion sin la lista de comidas que se encuentra en cola es mayor a 0, en caso contrario mostrara un mensaje que no se encuentran comidas para realizar
        /// </summary>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (this.comida != null)
            {
                this.comida.FinalizarPreparacion(this.hamburguesero.Nombre);
                this.rchFinalizados.Text += "\n" + comida.Ticket;
                this.comida = null;
            }
            else
            {
                MessageBox.Show("El Cocinero no posee comidas", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        /// <summary>
        /// El metodo realizara la serealizacion de las comidas que fue realizando el cocinero, guardandolo en un archivo de extension JSON
        /// </summary>
        private void FrmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Alumno: Serializar el cocinero antes de cerrar el formulario
            try
            {
                FileManager.Serealizar(this.hamburguesero, "cocinero.json");
            }
            catch(FileManagerException ex)
            {
                MessageBox.Show(ex.Message, "Error");
                FileManager.Guardar(ex.Message, "logs.txt", true);
            }
        }
    }
}