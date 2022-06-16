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
using CalculatorWPF.EventAggregation;
using CalculatorWPF.EventModels;
using System.Diagnostics;
using CalculatorWPF.Models;

namespace CalculatorWPF.ViewModels
{
    public class KeyboardViewModel : IViewModel
    {
        private ObservableCollection<KeyboardButton> _buttons = new ObservableCollection<KeyboardButton>();
        private readonly IEventAggregator _eventAggregator;

        public KeyboardViewModel()
        {
            _eventAggregator = Bootstrapper.Resolve<IEventAggregator>();

            var buttons = new List<KeyboardButton>()
            {
                new KeyboardButton{ Name = "C", ButtonClickedCommand = SetInstantOperation(InstantOperation.C) },
                new KeyboardButton{ Name = "CE", ButtonClickedCommand = SetInstantOperation(InstantOperation.CE) },
                new KeyboardButton{ Name = "Del", ButtonClickedCommand = SetInstantOperation(InstantOperation.DeleteLastFigure) },
                new KeyboardButton{ Name = "+", ButtonClickedCommand = SetOperation(Operation.Plus) },

                new KeyboardButton{ Name = "7", ButtonClickedCommand = PrintFigure("7") },
                new KeyboardButton{ Name = "8", ButtonClickedCommand = PrintFigure("8") },
                new KeyboardButton{ Name = "9", ButtonClickedCommand = PrintFigure("9") },
                new KeyboardButton{ Name = "-", ButtonClickedCommand = SetOperation(Operation.Minus) },

                new KeyboardButton{ Name = "4", ButtonClickedCommand = PrintFigure("4") },
                new KeyboardButton{ Name = "5", ButtonClickedCommand = PrintFigure("5") },
                new KeyboardButton{ Name = "6", ButtonClickedCommand = PrintFigure("6") },
                new KeyboardButton{ Name = "*", ButtonClickedCommand = SetOperation(Operation.Multiply) },

                new KeyboardButton{ Name = "1", ButtonClickedCommand = PrintFigure("1") },
                new KeyboardButton{ Name = "2", ButtonClickedCommand = PrintFigure("2") },
                new KeyboardButton{ Name = "3", ButtonClickedCommand = PrintFigure("3") },
                new KeyboardButton{ Name = "/", ButtonClickedCommand = SetOperation(Operation.Divide) },

                new KeyboardButton{ Name = "+/-", ButtonClickedCommand = SetInstantOperation(InstantOperation.ChangeSign) },
                new KeyboardButton{ Name = "0", ButtonClickedCommand = PrintFigure("0") },
                new KeyboardButton{ Name = ".", ButtonClickedCommand = PrintDot() },
                new KeyboardButton{ Name = "=", ButtonClickedCommand = SetInstantOperation(InstantOperation.Equals) },
            };

            foreach(var button in buttons)
            {
                Buttons.Add(button);
            }
        }

        private ICommand PrintDot()
        {
            return new RelayCommand((obj) =>
            {
                _eventAggregator.NotifyAsync(new KeyboardDotPressedEvent());
                Debug.WriteLine("debug-dot");
            });
        }

        private ICommand PrintFigure(string figure)
        {
            return new RelayCommand((obj) =>
            {
                _eventAggregator.NotifyAsync(new KeyboardFigurePressedEventModel(figure));
                Debug.WriteLine("debug");
            });
        }

        private ICommand SetOperation(Operation operation)
        {
            return new RelayCommand((obj) =>
            {
                _eventAggregator.NotifyAsync(new KeyboardOperationPressedEventModel(operation));
                Debug.WriteLine("debug-operation");
            });
        }

        private ICommand SetInstantOperation(InstantOperation instantOperation)
        {
            return new RelayCommand((obj) =>
            {
                _eventAggregator.NotifyAsync(new KeyboardInstantOperationPressedEventModel(instantOperation));
                Debug.WriteLine("debug-instant-operation");
            });
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

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public class KeyboardButton
        {
            public string Name { get; set; }
            public ICommand ButtonClickedCommand { get; set; }
        }
    }
}
