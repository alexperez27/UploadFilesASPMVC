using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DataProvider;
using WebApplication1.Models;
using System.Linq;
using System.Globalization;

namespace WebApplication1.Controllers
{
    public class AtletasController : Controller
    {
        private AtletasData dataProvider = new AtletasData();
        private string AtletasFolder = "/Archivos/";
        // GET: Atletas
        public ActionResult Index()
        {
            List<Atleta> atletas = dataProvider.GetAll();
            string fileController = GetBaseUrl();
            fileController += Url.Action("DownloadFile", "File");
            ViewBag.DownloadFileApi = fileController;
            //atletas.ForEach(a => a.RutaImagen = fileController + a.RutaImagen);
            return View(atletas);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Atleta());
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            Atleta item = dataProvider.GetByID(Id);
            string fileController = GetBaseUrl();
            fileController += Url.Action("DownloadFile", "File");
            //item.RutaImagen = fileController + item.RutaImagen;
            ViewBag.DownloadFileApi = fileController;
            return View("/Views/Atletas/Create.cshtml", item);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            Atleta item = dataProvider.GetByID(Id);
            string fileController = GetBaseUrl();
            fileController += Url.Action("DownloadFile", "File");
            //item.RutaImagen = fileController + item.RutaImagen;
            ViewBag.DownloadFileApi = fileController;
            return View(item);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            Resultado result = dataProvider.Delete(Id);
            if (result.Status)
                DeleteFile(result.Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(Atleta atleta)
        {
            if (atleta.Imagen == null && string.IsNullOrEmpty(atleta.RutaImagen))
            {
                ModelState.AddModelError("Imagen", "La imagen es requerida");
                return View(atleta);
            }

            if (atleta.Imagen != null)
            {
                string ImageName = System.IO.Path.GetFileName(atleta.Imagen.FileName);

                Resultado fileResult = SaveFile(atleta.Imagen, AtletasFolder);
                if (!fileResult.Status)
                {
                    ModelState.AddModelError("Imagen", "Surgió un error al guardar la imagen. Error:" + fileResult.Message);
                    return View(atleta);
                }

                atleta.RutaImagen = fileResult.Message;
                atleta.Mimetype = atleta.Imagen.ContentType;
                atleta.Tamnio = atleta.Imagen.ContentLength;
            }

            Resultado result = null;
            if (atleta.Id == 0)
                result = dataProvider.Add(atleta);
            else
                result = dataProvider.Edit(atleta);
            if (result.Status)
                return RedirectToAction("Index");
            return View(atleta);
        }

        private string GetBaseUrl()
        {
            string baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            return baseUrl;
        }

        private Resultado SaveFile(HttpPostedFileWrapper file, string subfolder)
        {
            try
            {
                string ImageName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                ImageName += "_" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_ss", CultureInfo.InvariantCulture);
                ImageName += System.IO.Path.GetExtension(file.FileName);
                ImageName = subfolder + ImageName;
                string physicalPath = Server.MapPath("~");

                Directory.CreateDirectory(physicalPath);

                physicalPath += ImageName;

                // save image in folder
                file.SaveAs(physicalPath);
                return new Resultado() { Status = true, Message = ImageName };
            }
            catch (Exception exception)
            {
                return new Resultado() { Status = false, Message = exception.Message };
            }
        }

        public void DeleteFile(string filePath)
        {
            string physicalPath = Server.MapPath("~");
            System.IO.File.Delete(physicalPath + filePath);
        }
    }
}