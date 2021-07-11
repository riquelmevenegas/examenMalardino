using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace WpfRiquelmeVenegas
{
    /// <summary>
    /// Lógica de interacción para WinRegistro.xaml
    /// </summary>
    public partial class WinRegistro : Window
    {
        int numOrden, codProducto, fechaCompra, cantidadUnidades;

        string strNumOrden, strCodProducto, strFechaCompra, strCantidadUnidades, producto;
        public WinRegistro()
        {
            InitializeComponent();
            CargaCombo();         
        }
        private void CargaCombo()
        {
            FileStream f;
            StreamReader rf;
            string linea;
            string[] campos;
            try
            {
                f = new FileStream("Productos.txt", FileMode.Open, FileAccess.Read);
                rf = new StreamReader(f);
                while (!rf.EndOfStream)
                {
                    linea = rf.ReadLine();
                    campos = linea.Split(';');
                    this.cboCodigoProducto.Items.Add(campos[1]);
                }
                rf.Close();
                f.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
        private void Limpiar()
        {
            this.txtNumeroOrden.Text = string.Empty;
            this.cboCodigoProducto.Items.Clear();
            this.txtFechaCompra.Text = string.Empty;
            this.txtCantidadUnidades.Text = string.Empty;          
        }
        private void ObtieneDatos()
        {           
            //Obtención de Número de Orden
            strNumOrden = this.txtNumeroOrden.Text;
            int.TryParse(strNumOrden, out numOrden);
            //Obtención de Código del Producto
            strCodProducto = this.cboCodigoProducto.Text;
            int.TryParse(strCodProducto, out codProducto);
            //Obtención de la Fecha de Compra
            strFechaCompra = this.txtFechaCompra.Text;
            int.TryParse(strFechaCompra, out fechaCompra);
            //Obtención de la Cantidad de Unidades
            strCantidadUnidades = this.txtCantidadUnidades.Text;
            int.TryParse(strCantidadUnidades, out cantidadUnidades);

        }
        private void Grabar()
        {
            FileStream f;
            StreamWriter wf;
            StringBuilder linea;
            try
            {
                f = new FileStream("Ordenes.txt", FileMode.Append, FileAccess.Write);
                wf = new StreamWriter(f);
                linea = new StringBuilder();
                linea.Append(strNumOrden);
                linea.Append(";");
                linea.Append(strFechaCompra);
                linea.Append(";");
                linea.Append(producto);
                linea.Append(";");
                linea.Append(strCantidadUnidades);

                wf.WriteLine(linea);
                wf.Close();
                f.Close();
                MessageBox.Show("Datos grabados!");
                Limpiar();
            }
            catch (IOException ex)
            {
                MessageBox.Show("ERROR " + ex.Message);
            }
        }
        private bool ValidarOrden()
        {
            bool estaOk = true;
            FileStream f;
            StreamReader rf;
            string linea;
            string[] campo;
            int stockActual, stockMinimo;
            try
            {
                f = new FileStream("Productos.txt", FileMode.Open, FileAccess.Read);
                rf = new StreamReader(f);
                while (!rf.EndOfStream)
                {
                    linea = rf.ReadLine();
                    campo = linea.Split(';');
                    int.TryParse(campo[3], out stockActual);
                    int.TryParse(campo[4], out stockMinimo);
                    if (stockActual > stockMinimo)
                    {
                        estaOk = false;
                    }       
                }
                rf.Close();
                f.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
            return estaOk;
        }
        private void BuscarProducto()
        {
            FileStream f;
            StreamReader rf;
            string linea;
            string[] campos;
            try
            {
                f = new FileStream("Productos.txt", FileMode.Open, FileAccess.Read);
                rf = new StreamReader(f);
                while (!rf.EndOfStream)
                {
                    linea = rf.ReadLine();
                    campos = linea.Split(';');
                    if (campos[1] == strCodProducto)
                    {
                        producto = campos[0];
                    }
                }
                rf.Close();
                f.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }
        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            ObtieneDatos();
            BuscarProducto();
            if (ValidarOrden())
            {
                MessageBox.Show("El stock actual aún no baja del minimo!");
            }
            else
            {
                Grabar();
            }
        }
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
