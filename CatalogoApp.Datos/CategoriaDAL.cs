using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CatalogoApp.Entidades;
using System.Security.Cryptography.X509Certificates;

namespace CatalogoApp.Datos
{
    public class CategoriaDAL
    {
        public List<Categoria> ListarTodos()
        {
            List<Categoria> categorias = new List<Categoria>();
            string query = "SELECT Id, Descricion FROM Categorias";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categoria categoria = new Categoria
                            {
                                Id = reader.GetInt32(0),
                                Descripcion = reader.GetString(1)
                            };
                            categorias.Add(categoria);
                        }
                    }
                }
                return categorias;
            }
        }
        public void Agregar(Categoria categoria)
        {
            string query = "INSERT INTO CATEGORIAS (Descripcion) VALUES (@descripcion)";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descripcion", categoria.Descripcion);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Modificar(Categoria categoria)
        {
            string query = "UPDATE CATEGORIAS SET Descripcion = @descripcion WHERE Id = @id";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@descripcion", categoria.Descripcion);
                    cmd.Parameters.AddWithValue("@id", categoria.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Eliminar(int id)
        {
            string query = "DELETE FROM CATEGORIAS WHERE Id = @id";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
