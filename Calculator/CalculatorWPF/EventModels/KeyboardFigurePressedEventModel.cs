using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.EventModels
{
    internal class KeyboardFigurePressedEventModel
    {
        public string Figure { get; private set; }

        public KeyboardFigurePressedEventModel(string figure)
        {
            Figure = figure;
        }
    }
}
