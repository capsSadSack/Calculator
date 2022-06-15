using CalculatorWPF.Models;

namespace CalculatorWPF.EventModels
{
    internal class KeyboardInstantOperationPressedEventModel
    {
        public InstantOperation InstantOperation { get; private set; }

        public KeyboardInstantOperationPressedEventModel(InstantOperation instantOperation)
        {
            InstantOperation = instantOperation;
        }
    }
}
