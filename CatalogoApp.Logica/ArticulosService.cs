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
            if (string.IsNullOrWhiteSpace(articulo.Codigo))
                throw new Exception("El codigo es obligatorio");
            if (string.IsNullOrWhiteSpace(articulo.Nombre))
                throw new Exception("el nombre es obligatorio");
            if (articulo.Precio <= 0)
                throw new Exception("el precio debe ser maayor a cero");
            if (articulo.Marca == null || articulo.Marca.Id <= 0)
                throw new Exception("debe seleccionar una marca");
            if (articulo.Categoria == null || articulo.Categoria.Id <= 0)
                throw new Exception("debe seleccionar una categoria");
            if(articulo.Id == 0)
                articuloDAL.Agregar(articulo);
            else
                articuloDAL.Modificar(articulo);
            if(articulo.Id != 0) { }
        }
        public void EliminarArticulo(int id)
        {
            if (id <= 0)
                throw new Exception("El ID esa invalido");
            articuloDAL.Eliminar(id);
        }
        public List<Articulo> Buscar(string criterio)
        {
            if (!string.IsNullOrWhiteSpace(criterio))
                return ObtenerTodos();

            return articuloDAL.Buscar(criterio);
        }
    }
}
