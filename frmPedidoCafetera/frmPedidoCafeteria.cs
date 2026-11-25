/*
 * ============================================================================
 * ESCUELA POLITÉCNICA NACIONAL
 * Asignatura: ITID 543 - Diseño y Programación de Software
 * Docente: Jose Naranjo
 * Estudiante: Paul Olmos
 * Evaluación: Examen Práctico Acumulativo - Primer Parcial
 * Archivo: frmPedidoCafeteria.cs
 * Descripción: Gestor Rápido de Pedidos con Catálogo - Cafetería Universitaria
 * ============================================================================
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace frmPedidoCafetera
{
    public partial class frmPedidoCafeteria : Form
    {
        // Almacena el precio unitario real del producto seleccionado desde lvCatalogo
        private decimal precioSeleccionado = 0.00m;

        public frmPedidoCafeteria()
        {
            InitializeComponent();
        }

        // ====================================================================
        // EVENTO 1: Load (frmPedidoCafeteria_Load)
        // Configura columnas, precarga los 10 productos oficiales e inicializa controles.
        // ====================================================================
        private void frmPedidoCafeteria_Load(object sender, EventArgs e)
        {
            // Configurar columnas de lvCatalogo
            lvCatalogo.Columns.Clear();
            lvCatalogo.Columns.Add("Producto", 160);
            lvCatalogo.Columns.Add("Precio", 70, HorizontalAlignment.Right);

            // Precarga exacta de los 10 productos oficiales (Sección 3)
            string[,] productosOficiales = new string[,]
            {
                { "Empanada de queso", "$1.25" },
                { "Sánduche de pollo", "$2.50" },
                { "Bolón de verde mixto", "$2.25" },
                { "Humita tradicional", "$1.50" },
                { "Tostada con queso", "$1.00" },
                { "Café pasado", "$0.85" },
                { "Chocolate caliente", "$1.35" },
                { "Jugo natural (naranja/mora)", "$1.50" },
                { "Batido de fruta", "$2.00" },
                { "Botella de agua", "$0.75" },
                { "Bebida energizante", "$1.25" }
            };

            lvCatalogo.Items.Clear();
            for (int i = 0; i < productosOficiales.GetLength(0); i++)
            {
                ListViewItem item = new ListViewItem(productosOficiales[i, 0]);
                item.SubItems.Add(productosOficiales[i, 1]);
                lvCatalogo.Items.Add(item);
            }

            // Configurar columnas de lvPedido
            lvPedido.Columns.Clear();
            lvPedido.Columns.Add("Producto", 130);
            lvPedido.Columns.Add("Cant.", 45, HorizontalAlignment.Center);
            lvPedido.Columns.Add("Subtotal", 68, HorizontalAlignment.Right);

            // Estado inicial de controles
            txtProducto.ReadOnly = true;
            txtCantidad.Text = "1";
            rbtnMostrador.Checked = true;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;
            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;
            lblTotal.Text = "Total: $0.00";
            lblEstado.Text = "Estado: Seleccione un producto.";
        }

        // ====================================================================
        // EVENTO 2: SelectedIndexChanged (lvCatalogo y lvPedido)
        // Carga datos del catálogo al seleccionar producto y habilita quitar en pedido.
        // ====================================================================
        private void lvCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCatalogo.SelectedItems.Count > 0)
            {
                ListViewItem fila = lvCatalogo.SelectedItems[0];
                string nombreProducto = fila.Text;
                string precioTexto = fila.SubItems[1].Text.Replace("$", "").Trim();

                if (decimal.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
                {
                    precioSeleccionado = precio;
                    txtProducto.Text = nombreProducto;
                    lblEstado.Text = $"Estado: '{nombreProducto}' seleccionado (${precioSeleccionado.ToString("0.00", CultureInfo.InvariantCulture)}).";
                    ValidarEstadoBotonAgregar();
                }
            }
        }

        private void lvPedido_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilita btnQuitar solo si existe una fila seleccionada en el carrito
            btnQuitar.Enabled = (lvPedido.SelectedItems.Count > 0);
        }

        // ====================================================================
        // EVENTO 3: Validating (txtCantidad_Validating)
        // Impide abandonar txtCantidad si el valor no es un entero >= 1.
        // ====================================================================
        private void txtCantidad_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad < 1)
            {
                e.Cancel = true;
                lblEstado.Text = "Error: La cantidad debe ser un entero mayor o igual a 1.";
                txtCantidad.SelectAll();
            }
        }

        // ====================================================================
        // EVENTO 4: TextChanged (txtCantidad_TextChanged y txtProducto_TextChanged)
        // Valida en tiempo real si se puede habilitar el botón Agregar Ítem.
        // ====================================================================
        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            ValidarEstadoBotonAgregar();
        }

        private void txtProducto_TextChanged(object sender, EventArgs e)
        {
            ValidarEstadoBotonAgregar();
        }

        // Método auxiliar para centralizar la habilitación de btnAgregar
        private void ValidarEstadoBotonAgregar()
        {
            bool productoValido = !string.IsNullOrWhiteSpace(txtProducto.Text);
            bool cantidadValida = int.TryParse(txtCantidad.Text.Trim(), out int cantidad) && cantidad >= 1;
            btnAgregar.Enabled = (productoValido && cantidadValida);
        }

        // ====================================================================
        // EVENTO 5: CheckedChanged (Modalidad de entrega y Extras)
        // Recalcula el total inmediatamente e informa en lblEstado.
        // ====================================================================
        private void rbtnMostrador_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnMostrador.Checked)
            {
                ActualizarTotal();
                lblEstado.Text = "Estado: Modalidad aplicada: Retiro en mostrador ($0.00).";
            }
        }

        private void rbtnEntrega_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEntrega.Checked)
            {
                ActualizarTotal();
                lblEstado.Text = "Estado: Opción aplicada: Entrega en aula (+$0.50).";
            }
        }

        private void chkPostre_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
            lblEstado.Text = chkPostre.Checked
                ? "Estado: Extra agregado: Postre del día (+$1.00)."
                : "Estado: Extra removido: Postre del día.";
        }

        private void chkServilletas_CheckedChanged(object sender, EventArgs e)
        {
            lblEstado.Text = chkServilletas.Checked
                ? "Estado: Extra aplicado: Servilletas / Cubiertos ($0.00)."
                : "Estado: Extra removido: Servilletas / Cubiertos.";
        }

        // ====================================================================
        // EVENTO 6: Click (btnAgregar_Click, btnQuitar_Click, btnLimpiar_Click)
        // Gestiona inserción, eliminación y reinicio completo del pedido.
        // ====================================================================
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // RF-04: Validaciones obligatorias antes de agregar
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                lblEstado.Text = "Error: Debe ingresar el nombre del cliente antes de agregar.";
                txtCliente.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProducto.Text) || precioSeleccionado <= 0)
            {
                lblEstado.Text = "Error: Seleccione un producto válido del catálogo.";
                return;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad < 1)
            {
                lblEstado.Text = "Error: La cantidad debe ser un entero mayor o igual a 1.";
                txtCantidad.Focus();
                txtCantidad.SelectAll();
                return;
            }

            // RF-02: Cálculo del subtotal del ítem
            decimal subtotal = cantidad * precioSeleccionado;
            string subtotalFormateado = "$" + subtotal.ToString("0.00", CultureInfo.InvariantCulture);

            // Agregar fila a lvPedido
            ListViewItem itemPedido = new ListViewItem(txtProducto.Text);
            itemPedido.SubItems.Add(cantidad.ToString());
            itemPedido.SubItems.Add(subtotalFormateado);
            lvPedido.Items.Add(itemPedido);

            // Recalcular total general y restablecer cantidad
            ActualizarTotal();
            lblEstado.Text = $"Estado: '{txtProducto.Text}' agregado (Cant: {cantidad}, Subtotal: {subtotalFormateado}).";
            txtCantidad.Text = "1";
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (lvPedido.SelectedItems.Count > 0)
            {
                string productoEliminado = lvPedido.SelectedItems[0].Text;
                lvPedido.Items.Remove(lvPedido.SelectedItems[0]);
                btnQuitar.Enabled = false;
                ActualizarTotal();
                lblEstado.Text = $"Estado: '{productoEliminado}' eliminado del pedido.";
            }
            else
            {
                lblEstado.Text = "Advertencia: Seleccione un ítem del pedido para quitar.";
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            lvPedido.Items.Clear();
            txtCliente.Clear();
            txtProducto.Clear();
            precioSeleccionado = 0.00m;
            lvCatalogo.SelectedItems.Clear();

            txtCantidad.Text = "1";
            rbtnMostrador.Checked = true;
            chkPostre.Checked = false;
            chkServilletas.Checked = false;

            btnAgregar.Enabled = false;
            btnQuitar.Enabled = false;
            lblTotal.Text = "Total: $0.00";
            lblEstado.Text = "Estado: Pedido reiniciado.";
            txtCliente.Focus();
        }

        // ====================================================================
        // MÉTODO DRY CENTRALIZADO (RF-05)
        // Recorre lvPedido, suma recargos/extras activos y actualiza lblTotal.
        // ====================================================================
        private void ActualizarTotal()
        {
            decimal sumaSubtotales = 0.00m;

            foreach (ListViewItem item in lvPedido.Items)
            {
                string subTexto = item.SubItems[2].Text.Replace("$", "").Trim();
                if (decimal.TryParse(subTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sub))
                {
                    sumaSubtotales += sub;
                }
            }

            decimal recargoEntrega = rbtnEntrega.Checked ? 0.50m : 0.00m;
            decimal extraPostre = chkPostre.Checked ? 1.00m : 0.00m;

            decimal totalGeneral = sumaSubtotales + recargoEntrega + extraPostre;
            lblTotal.Text = "Total: $" + totalGeneral.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}