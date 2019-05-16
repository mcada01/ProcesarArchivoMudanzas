using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;

namespace ProcesarArchivoMudanzas.Controllers
{
    [RoutePrefix("api/Mudanza")]
    public class MudanzaController : ApiController
    {
        [HttpGet]
        [Route("PostProcesarArchivo")]
        public int PostProcesarArchivo()
        {
            try
            {
                //if (HttpContext.Current.Request.Files.AllKeys.Any())
                //{
                //    // obtener el archivo
                //    var httpPostedFile = HttpContext.Current.Request.Files["UploadFile"];

                //    if (httpPostedFile != null)
                //    {
                //        //leer archivo y extraer los números   



                //        return Json(new { exitoso = true, mensaje = "User was created" });
                //    }
                //    else
                //    {
                //        return Json(new { exitoso = false, message = "El archivo no existe" });
                //    }
                //}
                //else
                //{
                //    return Json(new { exitoso = false, mensaje = "No se encontraron archivos" });
                //}
                //var linea1 = new List<int>() { 30, 30, 1, 1 };
                //var linea2 = new List<int>() { 20, 20, 20 };
                //var linea3 = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                //var linea4 = new List<int>() { 9, 19, 29, 39, 49, 59 };
                //var linea5 = new List<int>() { 32, 56, 76, 8, 44, 60, 47, 85, 71, 91 };

                //Console.WriteLine("Case #1: " + CalcularViajes(linea1));
                //Console.WriteLine("Case #2: " + CalcularViajes(linea2));
                //Console.WriteLine("Case #3: " + CalcularViajes(linea3));
                //Console.WriteLine("Case #4: " + CalcularViajes(linea4));
                //Console.WriteLine("Case #5: " + CalcularViajes(linea5));
                Console.ReadLine();
                return 1;
                //return Json(new { exitoso = false, mensaje = "No se encontraron archivos" });
            }
            catch (Exception ex)
            {
                //return Json(new { exitoso = false, mensaje = ex.Message });
                return 0;
            }
        }

        public static int CalcularViajes(List<int> elementos)
        {
            var pivot = elementos.Max(); //sacar el elemento de mayor peso
            elementos.Remove(pivot);

            var peso = 0;
            var i = 1; //numero de elementos a mover en la bolsa
            var viajes = 0;

            while (peso < 50 && pivot < 50)
            {
                if (elementos.Count == 0) //si no hay mas elementos
                    return 0;

                var masBajo = elementos.Min(); //sacar el elemento de menor peso
                elementos.Remove(masBajo);
                i++;
                peso = pivot * i;
            }

            viajes++;

            if (elementos.Count > 0)
                viajes += CalcularViajes(elementos);// invoca de nuevo la funcion hasta que no existan elementos

            return viajes;
        }

    }
}
