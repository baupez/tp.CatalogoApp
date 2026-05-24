using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogoApp.Datos;
using CatalogoApp.Entidades;

namespace CatalogoApp.Logica
{
    public class CategoriaService
    {
        private CategoriaDAL categoriaDAL =new CategoriaDAL();
        public List<Categoria> ObtenerTodos()
        {
            return categoriaDAL.ListarTodos();
        }
        public void GuardarCategoria(Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Descripcion))
                throw new Exception("La descripcion es obligatoria");
            if(categoria.Id == 0)
                categoriaDAL.Agregar(categoria);
            else
                categoriaDAL.Modificar(categoria);
        }
        public void EliminarCategoria(int id)
        {
            if (id <= 0)
                throw new Exception("El ID es invalido");
            categoriaDAL.Eliminar(id);
        }
    }
}
