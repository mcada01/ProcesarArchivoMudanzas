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

namespace ProcesarArchivoMudanzas.Controllers
{
    [RoutePrefix("api/Mudanza")]
    public class MudanzaController : ApiController
    {
        [HttpPost]
        [Route("PostProcesarArchivo")]
        public IHttpActionResult PostProcesarArchivo()
        {
            try
            {
                List<int> listadoInicial = new List<int>();

                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    // obtener el archivo publicado
                    var Archivo = HttpContext.Current.Request.Files["ArchivoSeleccionado"];

                    Stream fs = Archivo.InputStream;
                    var streamReader = new StreamReader(fs);
                    // leer el archivo
                    var result = streamReader.ReadToEnd();
                    result = result.Replace("\n", "");
                    var txt = result.Split('\r').ToList();
                    txt.Remove("");

                    listadoInicial = txt.Select(x => Convert.ToInt32(x)).ToList();
                    List<string> Resultado = new List<string>();
                    int DiaNumero = 0;
                    int c;
                    // construir un listado con el resultado que debe generar la salida
                    for (int z = 1; z < listadoInicial.Count; z++)
                    {
                        DiaNumero++;
                        var NumeroObjetos = listadoInicial[z];
                        List<int> listaPesoObjetosPorDia = new List<int>();

                        for (c = z+1; c <= (z+NumeroObjetos); c++)
                        {
                            listaPesoObjetosPorDia.Add(listadoInicial[c]);
                        }
                        
                        Resultado.Add("Case #" + DiaNumero + ": " + CalcularViajes(listaPesoObjetosPorDia));
                        z = c-1;
                    }
                    // crear el archivo y poblarlo
                    //StreamContent file;
                    using (StreamWriter miArchivo = new StreamWriter(@"C:\ArchivoSalida.txt"))
                    {
                        foreach (string linea in Resultado)
                        {
                            miArchivo.WriteLine(linea);
                        }
                        miArchivo.Close();
                        //file = new StreamContent(miArchivo.BaseStream);
                        
                    }

                    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(File.Open(@"C:\ArchivoSalida.txt",FileMode.Open,FileAccess.Read))
                    };
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "ArchivoSalida.txt"
                    };
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                    return responseMessageResult;
                    // descargar archivo generado
                    
                    //return Json(new { exitoso = false });
                }
                else
                {
                    return Json(new { exitoso = false, mensaje = "No se encontraron archivos" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { exitoso = false, mensaje = ex.Message });
                //return 0;
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
