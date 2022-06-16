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


        public bool TryCalculate(decimal number1, decimal number2, Operation operation, out decimal result)
        {
            Func<decimal, decimal, decimal> calcDelegate = CalculateAddition;

            switch (operation)
            {
                case Operation.Plus: calcDelegate = CalculateAddition; break;
                case Operation.Minus: calcDelegate = CaclulateSubtraction; break;
                case Operation.Multiply: calcDelegate = CaclulateMultiplication; break;
                case Operation.Divide: calcDelegate = CaclulateDivision; break;
            }

            try
            {
                result = calcDelegate(number1, number2);
                return true;
            }
            catch (Exception)
            {
                result = 0;
                return false;
            }
        }

        private decimal CalculateAddition(decimal number, decimal numberToAdd)
        {
            return number + numberToAdd;
        }

        private decimal CaclulateSubtraction(decimal number, decimal numberToSubtract)
        {
            return number - numberToSubtract;
        }

        private decimal CaclulateMultiplication(decimal number, decimal multiplier)
        {
            return number * multiplier;
        }

        private decimal CaclulateDivision(decimal number, decimal divider)
        {
            return number / divider;
        }
    }
}
