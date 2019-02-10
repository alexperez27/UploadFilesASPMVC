using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [RoutePrefix("File")]
    public class FileController : Controller
    {
        // GET: File
        [HttpGet]
        [Route("DownloadFile/{*FilePath}")]
        public ActionResult DownloadFile(string FilePath)
        {
            /*
             Para que el metodo acepte parametros como Carpeta1/Carpeta2/NombreArchivo -> se le especifica la ruta, y un asterisco en el parametro
             Para que el metodo acepte parametros con puntos, ej: Carpeta1/Carpeta2/NombreArchivo.png -> agregar en el archivo web.config:
             <system.webServer>
                <handlers>
                    <add name="ApiURIs-ISAPI-Integrated-4.0"
                        path="/File/*"
                        verb="GET,HEAD,POST,DEBUG,PUT,DELETE,PATCH,OPTIONS"
                        type="System.Web.Handlers.TransferRequestHandler"
                        preCondition="integratedMode,runtimeVersionv4.0" />
                </handlers>
            </system.webServer>
             */
            try
            {
                string physicalPath = Server.MapPath("~");
                FilePath = physicalPath + FilePath;

                if (!System.IO.File.Exists(FilePath))
                {
                    FilePath = physicalPath + "/Content/Images/not-found.png";
                    Response.StatusCode = 404;
                    if (!System.IO.File.Exists(FilePath))
                    {
                        return HttpNotFound("Archivo no encontrado");
                    }
                }
                byte[] fileBytes = System.IO.File.ReadAllBytes(FilePath);
                string mimeType = MimeMapping.GetMimeMapping(FilePath);
                string fileName = System.IO.Path.GetFileName(FilePath);
                return File(fileBytes, mimeType, fileName);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
    }
}