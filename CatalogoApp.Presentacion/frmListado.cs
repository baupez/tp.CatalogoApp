using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CatalogoApp.Entidades;
using CatalogoApp.Logica;

namespace CatalogoApp.Presentacion
{
    public partial class frmListado : Form
    {
        private ArticulosService articuloService;
        private BindingSource bindingSource;

        public frmListado()
        {
            InitializeComponent();
            articuloService = new ArticulosService();
            bindingSource = new BindingSource();
            ConfigurarDataGridView();
            CargarArticulos();
        }
        private void ConfigurarDataGridView()
        {
            dgvArticulos.AutoGenerateColumns = false;
            dgvArticulos.Columns.Clear();

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 50
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Codigo",
                HeaderText = "Código",
                DataPropertyName = "Codigo",
                Width = 100
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Nombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Width = 200
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Marca",
                HeaderText = "Marca",
                DataPropertyName = "Marca.Descripcion",
                Width = 100
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Categoria",
                HeaderText = "Categoría",
                DataPropertyName = "Categoria.Descripcion",
                Width = 100
            });

            dgvArticulos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Precio",
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "C2" }
            });
        }
        private void CargarArticulos()
        {
            try
            {
                var articulos = articuloService.ObtenerTodos();
                bindingSource.DataSource = articulos;
                dgvArticulos.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artículos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string criterio = txtBuscar.Text.Trim();
                var resultados = articuloService.Buscar(criterio);
                bindingSource.DataSource = resultados;
                dgvArticulos.DataSource = bindingSource;
                lblBuscar.Text = $"Resultados: {resultados.Count} artículos";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la búsqueda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmArticulos formArticulo = new frmArticulos();
            formArticulo.ShowDialog();
            CargarArticulos();
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un artículo para modificar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Articulo articulo = (Articulo)bindingSource.Current;
            frmArticulos formArticulo = new frmArticulos(articulo);
            formArticulo.ShowDialog();
            CargarArticulos();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un artículo para eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Articulo articulo = (Articulo)bindingSource.Current;
            DialogResult result = MessageBox.Show($"¿Eliminar el artículo {articulo.Nombre}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    articuloService.EliminarArticulo(articulo.Id);
                    CargarArticulos();
                    MessageBox.Show("Artículo eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
