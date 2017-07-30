using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.MVVMSQLite.Model;

namespace Xamarin.MVVMSQLite.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly DataBase database;

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string Idade { get; set; }

        public int Sexo { get; set; } = -1;

        public ObservableCollection<string> Records { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand DeleteAllCommand { get; set; }

        public ICommand SexoFilterCommand { get; set; }

        public ICommand IdadeFilterCommand { get; set; }

        public MainPageViewModel()
        {
            AddCommand = new Command(Add);
            DeleteCommand = new Command(Delete);
            DeleteAllCommand = new Command(DeleteAll);
            SexoFilterCommand = new Command(FilterBySexo);
            IdadeFilterCommand = new Command(FilterByIdade);
            database = new DataBase("Pessoas");
            database.CreateTable<Pessoa>();
            Records = new ObservableCollection<string>();
            ShowAllRecords();
        }

        void Add()
        {
            int idade;
            if (int.TryParse(Idade, out idade))
            {
                var record = new Pessoa() { Nome = Nome
                                          , Sobrenome = Sobrenome
                                          , Idade = idade
                                          , Sexo = Sexo == 0 ? Model.Sexo.Masculino : Model.Sexo.Feminino };

                database.SaveItem(record);

                Records.Add(record.ToString());
                OnPropertyChanged(nameof(Records));
                ClearForm();


            }
        }

        void Delete(object obj)
        {
            var itemString = (string) obj;
            var columns = itemString.Split(',').Select(i => i.Trim()).ToList();
            int id;
            if (int.TryParse(columns[0], out id))
            {
                database.DeleteItem<Pessoa>(id);
                Records.Remove((string) obj);
            }
        }

        void DeleteAll()
        {
            database.DeleteAll<Pessoa>();
            Records.Clear();
        }

        void FilterBySexo(object obj)
        {
            var sexo = ((string) obj) == "Feminino" ? Model.Sexo.Feminino : Model.Sexo.Masculino;

            var result = database.Query<Pessoa>("SELECT * FROM Pessoa WHERE Sexo = ?", new object[] {sexo});
            Records.Clear();
            foreach (var pessoa in result)
            {
                Records.Add(pessoa.ToString());
            }

        }

        void FilterByIdade(object obj)
        {
            int idade;

            if (int.TryParse((string) obj, out idade))
            {
                var result = database.Query<Pessoa>("SELECT * FROM Pessoa WHERE Idade >= ?", new object[] {idade});

                Records.Clear();
                foreach (var pessoa in result)
                {
                    Records.Add(pessoa.ToString());
                }
            }
        }




        void ClearForm()
        {
            Nome = string.Empty;
            Sobrenome = string.Empty;
            Idade = string.Empty;
            Sexo = -1;
            OnPropertyChanged(nameof(Nome));
            OnPropertyChanged(nameof(Sobrenome));
            OnPropertyChanged(nameof(Idade));
            OnPropertyChanged(nameof(Sexo));
        }

        void ShowAllRecords()
        {
            Records.Clear();
            var pessoas = database.GetItems<Pessoa>();
            foreach (var pessoa in pessoas)
            {
                Records.Add(pessoa.ToString());
            }
        }


    }
}
