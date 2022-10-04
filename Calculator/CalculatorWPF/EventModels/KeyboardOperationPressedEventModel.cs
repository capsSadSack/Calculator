using CalculatorWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.EventModels
{
    public class KeyboardOperationPressedEventModel
    {
        public Operation Operation { get; private set; }

        public KeyboardOperationPressedEventModel(Operation operation)
        {
            Operation = operation;
        }
    }
}
