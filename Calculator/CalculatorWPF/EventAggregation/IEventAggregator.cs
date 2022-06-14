using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.EventAggregation
{
    internal interface IEventAggregator
    {
        void Notify<T>(T message);

        void Subscribe<T>(AHandle<T> subscriber);

        void Unsubscribe<T>(AHandle<T> subscriber);
    }
}
