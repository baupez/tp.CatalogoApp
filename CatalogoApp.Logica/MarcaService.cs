using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogoApp.Datos;
using CatalogoApp.Entidades;

namespace CatalogoApp.Logica
{
    public class MarcaService
    {
        private MarcaDAL marcaDAL = new MarcaDAL();
        public List<Marca> ObtenerTodos()
        {  
            return new List<Marca>(); 
        }
        public void guardarMarca(Marca marca)
        {
            if (string.IsNullOrWhiteSpace(marca.Descripcion))
                throw new Exception("La descriocion es obligatoria");
            if(marca.Id == 0)
                marcaDAL.Agregar(marca);
            else
                marcaDAL.Modificar(marca);
        }
        public void EliminarMarca(int id)
        {
            if (id == 0)
                throw new Exception("el ID es invalido");
            marcaDAL.Eliminar(id);
        }
    }
}
