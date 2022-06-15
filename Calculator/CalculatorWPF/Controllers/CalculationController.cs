using CalculatorWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.Controllers
{
    internal class CalculationController
    {
        #region Singletone

        private static CalculationController _instance = null;

        private CalculationController()
        {

        }

        public static CalculationController GetInstance()
        {
            if(_instance == null)
            {
                _instance = new CalculationController();
            }

            return _instance;
        }

        #endregion

        public event EventHandler<string> OnExceptionOccur;

        public decimal Calculate(decimal number1, decimal number2, Operation operation)
        {
            Func<decimal, decimal, decimal> calcDelegate = CalculateAddition;

            switch (operation)
            {
                case Operation.Plus: calcDelegate = CalculateAddition; break;
                case Operation.Minus: calcDelegate = CaclulateSubtraction; break;
                case Operation.Multiply: calcDelegate = CaclulateMultiplication; break;
                case Operation.Divide: calcDelegate = CaclulateDivision; break;
            }

            return calcDelegate(number1, number2);
        }

        public decimal CalculateAddition(decimal number, decimal numberToAdd)
        {
            return number + numberToAdd;
        }

        public decimal CaclulateSubtraction(decimal number, decimal numberToSubtract)
        {
            return number - numberToSubtract;
        }

        public decimal CaclulateMultiplication(decimal number, decimal multiplier)
        {
            return number * multiplier;
        }

        public decimal CaclulateDivision(decimal number, decimal divider)
        {
            try
            {
                return number / divider;
            }
            catch
            {
                OnExceptionOccur?.Invoke(this, "");
                // TODO: [CG, 2022.06.15] Unexpected behaviour
                return 0;
            }
        }
    }
}
