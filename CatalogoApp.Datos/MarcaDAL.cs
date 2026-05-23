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
    public class MarcaDAL
    {
        public List<Marca> ListarTodos()
        {
            List<Marca> marcas = new List<Marca>();
            string query = "SELECT Id, Descricion FROM Marcas";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Marca marca = new Marca
                            {
                                Id = reader.GetInt32(0),
                                Descripcion = reader.GetString(1)
                            };
                            marcas.Add(marca);
                        }
                    }

                }
                return marcas;
            }
        }


        public void Agregar(Marca marca)
        {
            string query = "INSERT INTO Marcas (Descricion) VALUES (@Descripcion)";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descripcion", marca.Descripcion);
                    cmd.ExecuteNonQuery();

                }

            }

        }
        public void Modificar(Marca marca)
        {
            string query = "UPDATE MARCAS SET Descripcion = @descripcion WHERE Id = @id";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", marca.Id);
                    cmd.Parameters.AddWithValue("@descripcion", marca.Descripcion);
                    cmd.ExecuteNonQuery();

                }
            }
        }
    
        public void Eliminar(int id)
        {
            string query = "DELETE FROM MARCAS WHERE Id = @id";

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
