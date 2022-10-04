using CalculatorWPF.EventAggregation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IEventAggregator _eventAggregator;

        public MainViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Dispose()
        {
        }
    }
}
