using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorWPF.EventAggregation
{
    internal abstract class AHandle<T>
    {
        public AHandle(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        public abstract Task HandleAsync(T message, CancellationToken cancellationToken);
    }
}
