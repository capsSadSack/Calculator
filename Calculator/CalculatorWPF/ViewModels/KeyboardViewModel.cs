using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using CalculatorWPF.Commands;
using System.Windows.Input;

namespace CalculatorWPF.ViewModels
{
    internal class KeyboardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<KeyboardButton> _buttons = new ObservableCollection<KeyboardButton>()
        {
            new KeyboardButton{ Name = "C" },
            new KeyboardButton{ Name = "CE" },
            new KeyboardButton{ Name = "Del" },
            new KeyboardButton{ Name = "+" },

            new KeyboardButton{ Name = "7", ButtonClickedCommand = PrintFigure("7") },
            new KeyboardButton{ Name = "8", ButtonClickedCommand = PrintFigure("8") },
            new KeyboardButton{ Name = "9", ButtonClickedCommand = PrintFigure("9") },
            new KeyboardButton{ Name = "-" },

            new KeyboardButton{ Name = "4", ButtonClickedCommand = PrintFigure("4") },
            new KeyboardButton{ Name = "5", ButtonClickedCommand = PrintFigure("5") },
            new KeyboardButton{ Name = "6", ButtonClickedCommand = PrintFigure("6") },
            new KeyboardButton{ Name = "*" },

            new KeyboardButton{ Name = "1", ButtonClickedCommand = PrintFigure("1") },
            new KeyboardButton{ Name = "2", ButtonClickedCommand = PrintFigure("2") },
            new KeyboardButton{ Name = "3", ButtonClickedCommand = PrintFigure("3") },
            new KeyboardButton{ Name = "/" },

            new KeyboardButton{ Name = "+/-" },
            new KeyboardButton{ Name = "0", ButtonClickedCommand = PrintFigure("0") }, 
            new KeyboardButton{ Name = "." }, 
            new KeyboardButton{ Name = "=" },
        };

        private static ICommand PrintFigure(string figure)
        {
            // TODO: [CG, 2022.06.12] Заглушка
            return new RelayCommand((obj) => 
                Console.WriteLine("debug"));
        }

        public ObservableCollection<KeyboardButton> Buttons
        {
            get { return _buttons; }
            set
            {
                _buttons = value;
                OnPropertyChanged("Buttons");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public class KeyboardButton
        {
            public string Name { get; set; }
            public ICommand ButtonClickedCommand { get; set; }
        }

    }
}
