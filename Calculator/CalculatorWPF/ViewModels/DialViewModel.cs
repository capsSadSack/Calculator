using CalculatorWPF.EventAggregation;
using CalculatorWPF.EventModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorWPF.ViewModels
{
    internal class DialViewModel : 
        IViewModel, 
        IHandle<KeyboardFigurePressedEventModel>,
        IHandle<KeyboardOperationPressedEventModel>
    {
        private string _firstText = "0";
        public string FirstText 
        {
            get => _firstText;            
            set
            {
                _firstText = value;
                OnPropertyChanged("FirstText");
            }
        }

        private string _operationText = "";
        public string OperationText
        {
            get => _operationText;
            set
            {
                _operationText = value;
                OnPropertyChanged("OperationText");
            }
        }

        private string _secondText = "";
        public string SecondText
        {
            get => _secondText;
            set
            {
                _secondText = value;
                OnPropertyChanged("SecondText");
            }
        }

        private readonly IEventAggregator _eventAggregator;

        public DialViewModel()
        {
            _eventAggregator = Bootstrapper.Resolve<IEventAggregator>();

            _eventAggregator.Subscribe<KeyboardFigurePressedEventModel>(this);
            _eventAggregator.Subscribe<KeyboardOperationPressedEventModel>(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void Dispose()
        {
            
        }

        public void Handle(KeyboardOperationPressedEventModel message)
        {
            _isFirstFieldActive = false;
            _isOperationActive = true;
            _isSecondFieldActive = true;

            CurrentOperation = message.Operation;

            string operationText = "";
            switch (message.Operation)
            {
                case (Operation.Plus): operationText = "+"; break;
                case (Operation.Minus): operationText = "-"; break;
                case (Operation.Multiply): operationText = "*"; break;
                case (Operation.Divide): operationText = "/"; break;
            }
            OperationText = operationText;
        }

        public void Handle(KeyboardFigurePressedEventModel message)
        {
            if (_isFirstFieldActive)
            {
                FirstText = AddFigure(FirstText, message.Figure);
                _isOperationActive = true;
            }
            else if(_isSecondFieldActive)
            {
                SecondText = AddFigure(SecondText, message.Figure);
                _isOperationActive = false;
            }
        }

        private string AddFigure(string number, string figure)
        {
            if(number == "0")
            {
                return figure;
            }
            else
            {
                return number + figure;
            }
        }

        private bool _isFirstFieldActive = true;
        private bool _isSecondFieldActive = false;
        private bool _isOperationActive = false;
        private Operation? CurrentOperation = null;
    }
}
