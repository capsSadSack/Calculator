using CalculatorWPF.Controllers;
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
        private readonly CalculationController _calculationController;
        public DialViewModel()
        {
            _calculationController = CalculationController.GetInstance();

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

            _currentOperation = message.Operation;

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

            double.TryParse(FirstText, out double number1);
            double.TryParse(SecondText, out double number2);

            switch(message.InstantOperation)
            {            
                case InstantOperation.Equals: 
                    double result = _calculationController.Calculate(number1, number2, (Operation)_currentOperation);
                    SetState(State.OperationUnderResult);
                    SetTextFields(result.ToString(), "", "");
                    break;

                case InstantOperation.CE: 
                    SetState(State.FirstNumberEntering);
                    SetTextFields("0", "", "");
                    break;

                case InstantOperation.C:
                    if (_isFirstFieldActive)
                    {
                        SetState(State.FirstNumberEntering);
                        SetTextFields("0", "", "");
                    }
                    else if (_isSecondFieldActive && _isOperationActive == false)
                    {
                        SetState(State.ChangeOperationOrSecondNumberEntering);
                        SetTextFields(FirstText, OperationText, "");
                    }
                    else if (_isSecondFieldActive && _isOperationActive)
                    {
                        SetState(State.SecondNumberEntering);
                        SetTextFields(FirstText, OperationText, "");
                    }

                    break;

                case InstantOperation.DeleteLastFigure:
                    if(_isFirstFieldActive)
                    {
                        if(FirstText.Length > 0)
                        {
                            SetTextFields(RemoveLastFigure(FirstText), OperationText, SecondText);
                        }
                    }
                    if(_isSecondFieldActive)
                    {
                        if (SecondText.Length > 0)
                        {
                            SetTextFields(FirstText, OperationText, RemoveLastFigure(SecondText));
                        }
                    }

                    break;

                case InstantOperation.ChangeSign:

                    break;
            }
        }

        private string RemoveLastFigure(string number)
        {
            return number.Substring(0, number.Length - 1);
        }

        public void Handle(KeyboardFigurePressedEventModel message)
        {
            if (_isFirstFieldActive)
            {
                FirstText = AddFigure(FirstText, message.Figure);
                SetState(State.FirstNumberEntering);
            }
            else if(_isSecondFieldActive)
            {
                SecondText = AddFigure(SecondText, message.Figure);
                SetState(State.SecondNumberEntering);
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
        private Operation? _currentOperation = null;


        private void SetState(State state)
        {
            switch (state)
            {
                case State.FirstNumberEntering:
                    _isFirstFieldActive = true;
                    _isOperationActive = true;
                    _isSecondFieldActive = false;
                    break;

                case State.ChangeOperationOrSecondNumberEntering:
                    _isFirstFieldActive = false;
                    _isOperationActive = true;
                    _isSecondFieldActive = true;
                    break;

                case State.SecondNumberEntering:
                    _isFirstFieldActive = false;
                    _isOperationActive = false;
                    _isSecondFieldActive = true;
                    break;
                
                case State.OperationUnderResult:
                    _isFirstFieldActive = false;
                    _isOperationActive = true;
                    _isSecondFieldActive = false;
                    break;
            }
        }

        public void SetTextFields(string firstText, string operationText, string secondText)
        {
            FirstText = firstText;
            OperationText = operationText;
            SecondText = secondText;
        }

        private enum State
        {
            FirstNumberEntering,
            ChangeOperationOrSecondNumberEntering,
            SecondNumberEntering,
            OperationUnderResult
        }
    }
}
