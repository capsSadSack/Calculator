using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorWPF.EventAggregation
{
    public interface IEventAggregator
    {
        void Notify<T>(T message);

        void Subscribe<T>(IHandle<T> subscriber);

        void Unsubscribe<T>(IHandle<T> subscriber);
    }
}
