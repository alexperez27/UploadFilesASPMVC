using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.DataProvider
{
    public class AtletasData
    {
        public static List<Atleta> AtletasList = new List<Atleta>();

        public List<Atleta> GetAll()
        {
            return AtletasList;
        }

        public Atleta GetByID(int Id)
        {
            return AtletasList.Where(a => a.Id == Id).FirstOrDefault();
        }

        public Resultado Delete(int Id)
        {
            Atleta ToDelete = AtletasList.Where(a => a.Id == Id).FirstOrDefault();
            if (ToDelete == null)
                return new Resultado() { Status = false, Message = "No se encontro el elemento" };

            Resultado result = new Resultado();
            if (AtletasList.Remove(ToDelete))
            {
                result.Status = true;
                result.Message = ToDelete.RutaImagen;
            }
            else
            {
                result.Status = false;
                result.Message = "Surgio un error al borrar";
            }
            return result;
        }

        public Resultado Add(Atleta element)
        {
            element.Id = AtletasList.Count + 1;
            AtletasList.Add(element);
            return new Resultado() { Status = true, Message = "Atltea guardado exitosamene" };
        }

        public Resultado Edit(Atleta element)
        {
            Atleta ToEdit = AtletasList.Where(a => a.Id == element.Id).FirstOrDefault();
            if (ToEdit == null)
                return new Resultado() { Status = false, Message = "No se encontro el elemento" };

            ToEdit.Nombre = element.Nombre;
            ToEdit.RutaImagen = element.RutaImagen;
            ToEdit.Mimetype = element.Mimetype;
            ToEdit.Tamnio = element.Tamnio;

            return new Resultado() { Status = true, Message = "Atltea modificado exitosamene" };
        }
    }
}