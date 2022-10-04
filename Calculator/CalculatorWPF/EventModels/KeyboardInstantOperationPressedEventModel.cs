using CalculatorWPF.Models;

namespace CalculatorWPF.EventModels
{
    public class KeyboardInstantOperationPressedEventModel
    {
        public InstantOperation InstantOperation { get; private set; }

        public KeyboardInstantOperationPressedEventModel(InstantOperation instantOperation)
        {
            InstantOperation = instantOperation;
        }
    }
}
