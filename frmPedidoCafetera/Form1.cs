using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frmPedidoCafetera
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lvCatalogo.View = View.Details;
            lvCatalogo.GridLines = true;
            lvCatalogo.FullRowSelect = true;
            lvCatalogo.MultiSelect = true;


            lvPedido.View = View.Details;
            lvPedido.GridLines = true;
            lvPedido.FullRowSelect = true;
            lvPedido.MultiSelect = true;

            lvCatalogo.Items.Add("Empanada de queso", "1.25");
            lvCatalogo.Items.Add("Sanduche de pollo", "2.50");
            lvCatalogo.Items.Add("Bolón de verde Mixto", "2.25");
            lvCatalogo.Items.Add("Humita Tradicional", "1.50");
            lvCatalogo.Items.Add("Tostada de queso", "1.00");
            lvCatalogo.Items.Add("cafe pasado", "0.85");
            lvCatalogo.Items.Add("Chocolate caliente", "1.35");
            lvCatalogo.Items.Add("Jugo natural (naranja / mora)", "1.50");
            lvCatalogo.Items.Add("Batido de fruta", "2.00");
            lvCatalogo.Items.Add("Botella de agua", "0.75");
        }
        void CargarCatalogoOficial(string nombre, decimal precio)
        {

        }


        private void lvCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if(lvCatalogo.SelectedItems.Count > 0)
            {
                ListView item = lvCatalogo.SelectedItems[0];
                txtProducto.Text = item.Text;
                if (item.Tag is decimal precio){
                    precioSeleccionado = precio;
                }
                else
                {
                    string strPrecio = item.SubItems[1].Text.Replace("$", "").Trim();
                }
            } */

        }

        private void lvPedido_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {

        }

        private void rbtnMostrador_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {



        }

        private void rbtnEntrega_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
         /*   string cliente = txtCliente.Text.Trim();
            string producto = txtProducto.Text.Trim();
            if (string.IsNullOrWhiteSpace(cliente))
            {
                lblEstado.Text = "Error: Ingrese el nombre del cliente";
                txtCliente.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(producto) || precioSeleccionado <= 0)
            {
                lblEstado.Text = "Error: Debe seleccionar un producto del catálogo";
                return;
            }*/
        }
    }
}
