using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using ProcesarArchivoMudanzas.Database;
using ProcesarArchivoMudanzas.Models;

namespace ProcesarArchivoMudanzas.Controllers
{
    [RoutePrefix("api/Mudanza")]
    public class MudanzaController : ApiController
    {
        [HttpPost]
        [Route("PostProcesarArchivo/{id}")]
        public IHttpActionResult PostProcesarArchivo(int id)
        {
            try
            {
               
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // obtener el archivo publicado
                    var archivo = HttpContext.Current.Request.Files["ArchivoSeleccionado"];

                    Stream fs = archivo.InputStream;
                    var streamReader = new StreamReader(fs);

                    // leer el archivo
                    var result = streamReader.ReadToEnd();
                    result = result.Replace("\n", "");
                    var txt = result.Split('\r').ToList();
                    txt.Remove("");

                    var resultadoFinal = ProcesarDiasDeTrabajo(txt, id);

                    // descargar archivo generado
                    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        Content = new StringContent(resultadoFinal)
                    };
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "ArchivoSalida.txt"
                    };
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                    return responseMessageResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        private void GuardarLog(int id, string resultadoFinal)
        {
            using (var context = new MudanzasDbContext())
            {

                var log = new LogMudanza()
                {
                    Id = 0,
                    Documento = id,//asignar el documento que viene del front
                    FechaProceso = DateTime.Now,
                    NumeroViajes = resultadoFinal
                };

                context.LogMudanza.Add(log);
                context.SaveChanges();
            }
        }

        private string ProcesarDiasDeTrabajo(List<string> txt, int id)
        {
            List<int> listadoInicial = new List<int>();

            listadoInicial = txt.Select(x => Convert.ToInt32(x)).ToList();
            int diaNumero = 0;
            int c;

            string resultado = "";

            // construir un listado con el resultado que debe generar la salida
            for (int z = 1; z < listadoInicial.Count; z++)
            {
                diaNumero++;
                var numeroObjetos = listadoInicial[z];
                List<int> listaPesoObjetosPorDia = new List<int>();

                for (c = z + 1; c <= (z + numeroObjetos); c++)
                {
                    listaPesoObjetosPorDia.Add(listadoInicial[c]);
                }

                var resultadoxDia = "Case #" + diaNumero + ": " + CalcularViajes(listaPesoObjetosPorDia);

                resultado = string.Concat(resultado, resultadoxDia , Environment.NewLine);

                GuardarLog(id, resultadoxDia);

                z = c - 1;
            }

            return resultado;
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
