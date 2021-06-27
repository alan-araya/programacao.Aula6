using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SoapWebService;
using System.Text.Json;
using Refit;

namespace ConsoleApp.Aula6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Aula 6! Consumindo Web Services e APIs");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Consumindo via SOAP");
            Console.WriteLine("--------------------------------------------------------");

            //Incializa o client
            var soapClient = new WebServiceSoapAulaSoapClient(WebServiceSoapAulaSoapClient.EndpointConfiguration.WebServiceSoapAulaSoap);
            
            var retornoFibonnaci = await soapClient.CalculaFibonnaciAsync(10);

            Console.WriteLine("Resultado Fibonnaci:");
            foreach (int numero in retornoFibonnaci.Body.CalculaFibonnaciResult)
            {
                Console.Write($"{numero},");
            }
            //output no console:
            //1,2,3,5,8,13,21,34,55,

            var retornoAutores = await soapClient.GetRetornoComplexoAsync("Intersaberes, O'Relly, Meaning, B2you");

            Console.WriteLine("");
            Console.WriteLine("Resultado Autores:");
            foreach (var autor in retornoAutores.Body.GetRetornoComplexoResult)
            {
                Console.WriteLine($"{autor.Nome} - {autor.Editora} - {autor.DataNascimento}");
            }
            //Output no console:
            //Resultado Autores:
            //JOSIANE - Meaning - 25 / 06 / 1965 00:00:00
            //GABRIEL - O'Relly - 29/04/1973 00:00:00
            //JOSIANE - Intersaberes - 30 / 11 / 1982 00:00:00
            //EDUARDO - B2you - 09 / 10 / 1962 00:00:00
            //MARIA - O'Relly - 07/10/1998 00:00:00
            //JOSIANE - Intersaberes - 19 / 03 / 1981 00:00:00


            //---------------------------------------------------------
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Consumindo via REST");
            Console.WriteLine("--------------------------------------------------------");


            var httpClient = new HttpClient();
            int numeroFibonnaci = 10;
            HttpResponseMessage responseFibonnaci = await httpClient.GetAsync($"http://localhost:5000/aula/fibonnaci?numero={numeroFibonnaci}");

            if (responseFibonnaci.IsSuccessStatusCode)
            {
                string contentString = await responseFibonnaci.Content.ReadAsStringAsync();
                var sequenciaFibonnaci = System.Text.Json.JsonSerializer.Deserialize<int[]>(contentString);

                foreach (int numero in sequenciaFibonnaci)
                {
                    Console.Write($"{numero},");
                }
                //output no console:
                //1,2,3,5,8,13,21,34,55,
            }


            string editoras = "Intersaberes, O'Relly, Meaning, B2you";
            HttpResponseMessage responseAutores = await httpClient.GetAsync($"http://localhost:5000/aula/autores?editoras={editoras}");

            if (responseFibonnaci.IsSuccessStatusCode)
            {
                string contentString = await responseAutores.Content.ReadAsStringAsync();
                var autores = System.Text.Json.JsonSerializer.Deserialize<List<Autor>>(contentString, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                Console.WriteLine("");
                Console.WriteLine("Resultado Autores:");
                foreach (var autor in autores)
                {
                    Console.WriteLine($"{autor.Nome} - {autor.Editora} - {autor.DataNascimento}");
                }
                //output no console:
                //1,2,3,5,8,13,21,34,55,
            }

            ExemplosSerializacao();

            await ExemplosRefit();
        }


        private static void ExemplosSerializacao() 
        {
            var autor = new Autor()
            {
                Codigo = 100,
                Nome = "JON Author",
                DataNascimento = new DateTime(1979, 6, 20),
                Editora = "Intersaberes",
                EnderecoAutor = new Endereco()
                {
                    CEP = "81560-120",
                    Complemento = "AP 20",
                    Numero = 2021,
                    Rua = "Rua de Teste",
                    PossuiPortaria = true
                }
            };
            var autor2 = new Autor()
            {
                Codigo = 200,
                Nome = "POP Author",
                DataNascimento = new DateTime(1999, 2, 10),
                Editora = "Intersaberes",
                EnderecoAutor = new Endereco()
                {
                    CEP = "81550-110",
                    Rua = "Av. dos Estados",
                    PossuiPortaria = false
                }
            };
            var autores = new List<Autor>() { autor, autor2 };

            var autor1String = JsonSerializer.Serialize(autor);

            var autorDeseriazado = JsonSerializer.Deserialize<Autor>(autor1String);

            var autoresString = JsonSerializer.Serialize(autores);
        }

        private static async Task ExemplosRefit() 
        {
            //Cria o client com base na interface
            var refitClient = RestService.For<IWebApiAulaRefit>("http://localhost:5000/aula");

            //executa o método "GET fibonnaci" da web api
            var reultadoFibonnaci = await refitClient.CalculaFibonnacciAsync(10);

            //executa o método "GET autores" da web api
            var autores = await refitClient.GetAutoresAsync("Intersaberes, O'Relly, Meaning, B2you");
        }
    }
}
