using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CatalogoApp.Entidades;
using CatalogoApp.Logica;

namespace CatalogoApp.Presentacion
{
    public partial class frmArticulos : Form
    {
        private ArticulosService articuloService;
        private Articulo articuloActual;
        private List<string> listaImagenes;
        public frmArticulos()
        {
            InitializeComponent();
            articuloService = new ArticulosService();
            articuloActual = new Articulo();
            listaImagenes = new List<string>();
            CargarCombos();
        }
        public frmArticulos(Articulo articulo)
        {
            InitializeComponent();
            articuloService = new ArticulosService();
            articuloActual = articulo;
            listaImagenes = new List<string>();
            if (articuloActual.Imagenes != null)
            {
                foreach (var img in articuloActual.Imagenes)
                {
                    listaImagenes.Add(img.ImagenUrl);
                }
            }
            CargarCombos();
            CargarDatosEnFormulario();
            ActualizarListaImagenes();
        }
        private void CargarCombos()
        {
            try
            {
                var marcas = articuloService.ObtenerMarcas();
                cmbMarca.DataSource = null;
                cmbMarca.DataSource = marcas;
                cmbMarca.DisplayMember = "Descripcion";
                cmbMarca.ValueMember = "Id";
                var categorias = articuloService.ObtenerCategorias();
                cmbCategoria.DataSource = null;
                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "Descripcion";
                cmbCategoria.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar combos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarDatosEnFormulario()
        {
            txtCodigo.Text = articuloActual.Codigo;
            txtNombre.Text = articuloActual.Nombre;
            txtDescripcion.Text = articuloActual.Descripcion;
            txtPrecio.Text = articuloActual.Precio.ToString("0.00");
            if (articuloActual.Marca != null)
                cmbMarca.SelectedValue = articuloActual.Marca.Id;
            if (articuloActual.Categoria != null)
                cmbCategoria.SelectedValue = articuloActual.Categoria.Id;
        }
        private void ActualizarListaImagenes()
        {
            lstImagenes.DataSource = null;
            lstImagenes.DataSource = listaImagenes;
        }
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            string url = txtImagenUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Ingrese una URL de imagen", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!listaImagenes.Contains(url))
            {
                listaImagenes.Add(url);
                ActualizarListaImagenes();
                txtImagenUrl.Clear();
                txtImagenUrl.Focus();
            }
            else
            {
                MessageBox.Show("La imagen ya está en la lista", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnQuitarImagen_Click(object sender, EventArgs e)
        {
            if (lstImagenes.SelectedItem != null)
            {
                string url = lstImagenes.SelectedItem.ToString();
                listaImagenes.Remove(url);
                ActualizarListaImagenes();
            }
            else
            {
                MessageBox.Show("Seleccione una imagen para quitar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCodigo.Text))
                {
                    MessageBox.Show("El código es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCodigo.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    MessageBox.Show("El precio es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }
                decimal precio;
                if (!decimal.TryParse(txtPrecio.Text, out precio))
                {
                    MessageBox.Show("El precio debe ser un número válido", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrecio.Focus();
                    return;
                }
                if (cmbMarca.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione una marca", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cmbCategoria.SelectedItem == null)
                {
                    MessageBox.Show("Seleccione una categoría", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                articuloActual.Codigo = txtCodigo.Text;
                articuloActual.Nombre = txtNombre.Text;
                articuloActual.Descripcion = txtDescripcion.Text;
                articuloActual.Precio = precio;
                articuloActual.Marca = (Marca)cmbMarca.SelectedItem;
                articuloActual.Categoria = (Categoria)cmbCategoria.SelectedItem;
                articuloActual.Imagenes.Clear();
                foreach (string url in listaImagenes)
                {
                    articuloActual.Imagenes.Add(new Imagen { ImagenUrl = url });
                }
                articuloService.GuardarArticulo(articuloActual);
                MessageBox.Show("Artículo guardado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    } 
}
