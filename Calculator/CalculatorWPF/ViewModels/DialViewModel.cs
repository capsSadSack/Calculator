using CalculatorWPF.EventAggregation;
using CalculatorWPF.EventModels;
using CalculatorWPF.Models;
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
        IHandle<KeyboardDotPressedEvent>,
        IHandle<KeyboardFigurePressedEventModel>,
        IHandle<KeyboardOperationPressedEventModel>,
        IHandle<KeyboardInstantOperationPressedEventModel>
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

            _eventAggregator.Subscribe<KeyboardDotPressedEvent>(this);
            _eventAggregator.Subscribe<KeyboardFigurePressedEventModel>(this);
            _eventAggregator.Subscribe<KeyboardOperationPressedEventModel>(this);
            _eventAggregator.Subscribe<KeyboardInstantOperationPressedEventModel>(this);
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

            FirstText = UpdateNumber(FirstText);
        }

        private string UpdateNumber(string text)
        {
            string updatedNumber = double.Parse(text).ToString();
            return updatedNumber;
        }

        public void Handle(KeyboardDotPressedEvent message)
        {
            if (_isFirstFieldActive)
            {
                FirstText = AddDot(FirstText);
                _isOperationActive = true;
            }
            else if (_isSecondFieldActive)
            {
                SecondText = AddDot(SecondText);
                _isOperationActive = false;
            }

        }

        public void Handle(KeyboardInstantOperationPressedEventModel message)
        {
            _isFirstFieldActive = false;
            _isOperationActive = true;
            _isSecondFieldActive = true;

            SecondText = "";
            OperationText = "";
            CurrentOperation = null;

            // TODO: [CG, 2022.06.15] Call controller
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

        private string AddDot(string number)
        {
            if (CountDotsInNumber(number) == 0)
            {
                if (number == "")
                {
                    return "0.";
                }
                else
                {
                    return number + '.';
                }
            }
            else
            {
                return number;
            }
        }

        private int CountDotsInNumber(string number)
        {
            int counter = 0;

            for(int i = 0; i < number.Length; i++)
            {
                if (number[i] == '.')
                {
                    counter++;
                }
            }

            return counter;
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
