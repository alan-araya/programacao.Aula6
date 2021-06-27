using System;

namespace ConsoleApp.Aula6
{
    public class Autor
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Editora { get; set; }
        public DateTime DataNascimento { get; set; }

        public Endereco EnderecoAutor { get; set; }
    }
}
