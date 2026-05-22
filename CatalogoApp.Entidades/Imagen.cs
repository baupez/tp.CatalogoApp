using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoApp.Entidades
{
    internal class Imagen
    {
        public int Id { get; set; }
        public int idArticulo { get; set; }
        public string ImagenUrl { get; set; }
    }
}
