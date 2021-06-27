using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace ConsoleApp.Aula6
{
    interface IWebApiAulaRefit
    {
        [Get("/fibonnaci")]
        public Task<List<int>> CalculaFibonnacciAsync(int numero);

        [Get("/autores")]
        public Task<IEnumerable<Autor>> GetAutoresAsync([AliasAs("editoras")] string editorasSeparadoPorVirgula);
    }
}
