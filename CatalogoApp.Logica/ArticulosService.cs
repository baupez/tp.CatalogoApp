using CatalogoApp.Datos;
using CatalogoApp.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoApp.Logica
{
    public class ArticulosService
    {
        private ArticuloDAL articuloDAL = new ArticuloDAL();
        private MarcaDAL marcaDAL = new MarcaDAL();
        private CategoriaDAL categoriaDAL = new CategoriaDAL();
        public List<Articulo> ObtenerTodos()
        {
            return articuloDAL.ListarTodos();
        }
        public List<Marca> ObtenerMarcas()
        {
            return marcaDAL.ListarTodos();
        }
        public List<Categoria> ObtenerCategorias()
        {
            return categoriaDAL.ListarTodos();
        }
        public void GuardarArticulo(Articulo articulo)
        {
        }
    }
}
