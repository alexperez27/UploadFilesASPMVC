using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Atleta
    {
        public int Id { get; set; }

        [DisplayName(displayName: "Nombre del atleta")]
        [Required(ErrorMessage = "El nombre del atleta es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La imagen es requerido")]
        public string RutaImagen { get; set; }

        public HttpPostedFileWrapper Imagen { get; set; }

        public string Mimetype { get; set; }

        public long Tamnio { get; set; }
    }
}