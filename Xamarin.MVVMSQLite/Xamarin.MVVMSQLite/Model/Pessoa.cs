using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.MVVMSQLite.Model
{

    public enum Sexo
    {
        Masculino,
        Feminino
    }

    public class Pessoa:BaseItem
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }

        public int Idade { get; set; }

        public Sexo Sexo { get; set; }

        public override string ToString()
        {
            return $"{ID}, {Nome}, {Sobrenome}, {Idade}, {Sexo.ToString()}";
        }
    }
}
