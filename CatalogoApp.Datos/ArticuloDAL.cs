using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CatalogoApp.Entidades;

namespace CatalogoApp.Datos
{
    public class ArticuloDAL
    {
        public List<Articulo> ListarTodos()
        {
            List<Articulo> articulos = new List<Articulo>();
            string query = @"SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.Precio,
                            m.Id as MarcaId, m.Descripcion as MarcaDesc,
                            c.Id as CategoriaId, c.Descripcion as CategoriaDesc
                     FROM ARTICULOS a
                     LEFT JOIN MARCAS m ON a.IdMarca = m.Id
                     LEFT JOIN CATEGORIAS c ON a.IdCategoria = c.Id
                     ORDER BY a.Nombre";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Articulo articulo = new Articulo
                        {
                            Id = reader.GetInt32(0),
                            Codigo = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            Descripcion = reader.GetString(3),
                            Precio = reader.GetDecimal(4),
                            Marca = new Marca
                            {
                                Id = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                Descripcion = reader.IsDBNull(6) ? "" : reader.GetString(6)
                            },
                            Categoria = new Categoria
                            {
                                Id = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                Descripcion = reader.IsDBNull(8) ? "" : reader.GetString(8)
                            }
                        };
                        articulo.Imagenes = ObtenerImagenesPorArticulo(articulo.Id);
                        articulos.Add(articulo);
                    }
                }
            }
            return articulos;
        }
        public List<Imagen> ObtenerImagenesPorArticulo(int idArticulo)
        {
            List<Imagen> imagenes = new List<Imagen>();
            string query = "SELECT Id, IdArticulo, ImagenUrl FROM IMAGENES WHERE IdArticulo = @id";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idArticulo);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Imagen imagen = new Imagen
                            {
                                Id = reader.GetInt32(0),
                                idArticulo = reader.GetInt32(1),
                                ImagenUrl = reader.GetString(2)
                            };
                            imagenes.Add(imagen);
                        }
                    }
                }
            }
            return imagenes;
        }
        public void Agregar(Articulo articulo)
        {
            string query = @"INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) 
                     VALUES (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @precio);
                     SELECT SCOPE_IDENTITY()";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", articulo.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                    cmd.Parameters.AddWithValue("@idMarca", articulo.Marca?.Id ?? 0);
                    cmd.Parameters.AddWithValue("@idCategoria", articulo.Categoria?.Id ?? 0);
                    cmd.Parameters.AddWithValue("@precio", articulo.Precio);

                    int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
                    articulo.Id = nuevoId;

                    foreach (var img in articulo.Imagenes)
                    {
                        AgregarImagen(nuevoId, img.ImagenUrl);
                    }
                }
            }
        }
        public void AgregarImagen(int idArticulo, string url)
        {
            string query = "INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @url)";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idArticulo", idArticulo);
                    cmd.Parameters.AddWithValue("@url", url);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Modificar(Articulo articulo)
        {
            string query = @"UPDATE ARTICULOS SET 
                     Codigo = @codigo, 
                     Nombre = @nombre, 
                     Descripcion = @descripcion, 
                     IdMarca = @idMarca, 
                     IdCategoria = @idCategoria, 
                     Precio = @precio 
                     WHERE Id = @id";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", articulo.Id);
                    cmd.Parameters.AddWithValue("@codigo", articulo.Codigo);
                    cmd.Parameters.AddWithValue("@nombre", articulo.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", articulo.Descripcion);
                    cmd.Parameters.AddWithValue("@idMarca", articulo.Marca?.Id ?? 0);
                    cmd.Parameters.AddWithValue("@idCategoria", articulo.Categoria?.Id ?? 0);
                    cmd.Parameters.AddWithValue("@precio", articulo.Precio);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Eliminar(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string deleteImagenes = "DELETE FROM IMAGENES WHERE IdArticulo = @id";
                using (SqlCommand cmd = new SqlCommand(deleteImagenes, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                string deleteArticulo = "DELETE FROM ARTICULOS WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(deleteArticulo, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Articulo> Buscar(string criterio)
        {
            List<Articulo> articulos = new List<Articulo>();

            string query = @"SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.Precio,
                                    m.Id as MarcaId, m.Descripcion as MarcaDesc,
                                    c.Id as CategoriaId, c.Descripcion as CategoriaDesc
                             FROM ARTICULOS a
                             LEFT JOIN MARCAS m ON a.IdMarca = m.Id
                             LEFT JOIN CATEGORIAS c ON a.IdCategoria = c.Id
                             WHERE a.Codigo LIKE @criterio 
                                OR a.Nombre LIKE @criterio 
                                OR a.Descripcion LIKE @criterio
                             ORDER BY a.Nombre";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@criterio", "%" + criterio + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articulo articulo = new Articulo
                            {
                                Id = reader.GetInt32(0),
                                Codigo = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Descripcion = reader.GetString(3),
                                Precio = reader.GetDecimal(4),
                                Marca = new Marca
                                {
                                    Id = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                    Descripcion = reader.IsDBNull(6) ? null : reader.GetString(6)
                                },
                                Categoria = new Categoria
                                {
                                    Id = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                    Descripcion = reader.IsDBNull(8) ? null : reader.GetString(8)
                                }
                            };
                            articulo.Imagenes = ObtenerImagenesPorArticulo(articulo.Id);
                            articulos.Add(articulo);
                        }
                    }
                }
            }
            return articulos;
        }
    }
}

