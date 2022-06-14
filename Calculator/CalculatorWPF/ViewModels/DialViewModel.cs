using CalculatorWPF.EventAggregation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.ViewModels
{
    internal class DialViewModel : INotifyPropertyChanged
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


        //private readonly IEventAggregator _eventAggregator;

        //public DialViewModel(IEventAggregator eventAggregator)
        //{
        //    _eventAggregator = eventAggregator;
        //}

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
