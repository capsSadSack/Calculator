﻿using CalculatorWPF.Controllers;
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
    public class DialViewModel : 
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

        #region +, -, *, / pressed

        public void Handle(KeyboardOperationPressedEventModel message)
        {
            SetState(State.ChangeOperationOrSecondNumberEntering);

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

        public async Task HandleAsync(KeyboardOperationPressedEventModel message, CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => Handle(message), cancellationToken);
            await task;
        }

        private string UpdateNumber(string text)
        {
            if (decimal.TryParse(text, out decimal number))
            {
                return number.ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region . pressed

        public void Handle(KeyboardDotPressedEvent message)
        {
            if (_currentState == State.FirstNumberEntering)
            {
                FirstText = AddDot(FirstText);
                _isOperationActive = true;
            }
            else if (_currentState == State.SecondNumberEntering ||
                _currentState == State.ChangeOperationOrSecondNumberEntering)
            {
                SecondText = AddDot(SecondText);
                _isOperationActive = false;
            }
        }

        public async Task HandleAsync(KeyboardDotPressedEvent message, CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => Handle(message), cancellationToken);
            await task;
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

            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] == '.')
                {
                    counter++;
                }
            }

            return counter;
        }

        #endregion

        #region =, C, CE, Del, [+/-] pressed

        public void Handle(KeyboardInstantOperationPressedEventModel message)
        {
            decimal.TryParse(FirstText, out decimal number1);
            decimal.TryParse(SecondText, out decimal number2);

            switch (message.InstantOperation)
            {
                case InstantOperation.Equals:
                    {
                        if (_calculationController.TryCalculate(number1, number2, (Operation)_currentOperation,
                            out decimal result))
                        {
                            SetState(State.OperationUnderResult);
                            SetTextFields(result.ToString(), "", "");
                        }
                        else
                        {
                            SetState(State.FirstNumberEntering);
                            SetTextFields("Error", "", "");
                        }
                        
                        break;
                    }

                case InstantOperation.CE:
                    {
                        SetState(State.FirstNumberEntering);
                        SetTextFields("0", "", "");
                        break;
                    }

                case InstantOperation.C:
                    {
                        if (_currentState == State.FirstNumberEntering ||
                            _currentState == State.OperationUnderResult)
                        {
                            SetState(State.FirstNumberEntering);
                            SetTextFields("0", "", "");
                        }
                        else if (_currentState == State.SecondNumberEntering)
                        {
                            SetState(State.ChangeOperationOrSecondNumberEntering);
                            SetTextFields(FirstText, OperationText, "");
                        }
                        else if (_currentState == State.ChangeOperationOrSecondNumberEntering)
                        {
                            SetState(State.SecondNumberEntering);
                            SetTextFields(FirstText, OperationText, "");
                        }

                        break;
                    }

                case InstantOperation.DeleteLastFigure:
                    {
                        if (_currentState == State.FirstNumberEntering)
                        {
                            if (FirstText.Length > 0)
                            {
                                SetTextFields(RemoveLastFigure(FirstText), OperationText, SecondText);
                            }
                        }
                        if (_currentState == State.SecondNumberEntering ||
                            _currentState == State.ChangeOperationOrSecondNumberEntering)
                        {
                            if (SecondText.Length > 0)
                            {
                                SetTextFields(FirstText, OperationText, RemoveLastFigure(SecondText));
                            }
                        }

                        break;
                    }

                case InstantOperation.ChangeSign:
                    {
                        if (_currentState == State.FirstNumberEntering ||
                            _currentState == State.OperationUnderResult)
                        {
                            SetTextFields(InverseSign(FirstText), OperationText, SecondText);
                        }
                        if (_currentState == State.SecondNumberEntering ||
                            _currentState == State.ChangeOperationOrSecondNumberEntering)
                        {
                            SetTextFields(FirstText, OperationText, InverseSign(SecondText));
                        }
                        break;
                    }
            }
        }

        public async Task HandleAsync(KeyboardInstantOperationPressedEventModel message, CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => Handle(message), cancellationToken);
            await task;
        }

        private string InverseSign(string number)
        {
            if (number.Length == 0)
            {
                return "";
            }

            if (number == "0")
            {
                return number;
            }

            if (number.StartsWith("-"))
            {
                return number.Substring(1);
            }
            else
            {
                return "-" + number;
            }
        }

        private string RemoveLastFigure(string number)
        {
            return number.Substring(0, number.Length - 1);
        }

        #endregion

        #region Figure (0, 1, 2, 3, ..., 9) pressed

        public void Handle(KeyboardFigurePressedEventModel message)
        {
            if (_currentState == State.FirstNumberEntering)
            {
                FirstText = AddFigure(FirstText, message.Figure);
                SetState(State.FirstNumberEntering);
            }
            else if(_currentState == State.SecondNumberEntering ||
                    _currentState == State.ChangeOperationOrSecondNumberEntering)
            {
                SecondText = AddFigure(SecondText, message.Figure);
                SetState(State.SecondNumberEntering);
            }
        }

        public async Task HandleAsync(KeyboardFigurePressedEventModel message, CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => Handle(message), cancellationToken);
            await task;
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

        #endregion

        private bool _isFirstFieldActive = true;
        private bool _isSecondFieldActive = false;
        private bool _isOperationActive = false;
        private Operation? _currentOperation = null;

        private State _currentState = State.FirstNumberEntering;

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

            _currentState = state;
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
