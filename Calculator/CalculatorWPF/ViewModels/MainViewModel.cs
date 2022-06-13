using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.ViewModels
{
    internal class MainViewModel : IMainViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Dispose()
        {
        }
    }
}
