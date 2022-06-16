using System.Threading;
using System.Threading.Tasks;

namespace CalculatorWPF.EventAggregation
{
    public interface IHandle<T>
    {
        void Handle(T message);

        Task HandleAsync(T message, CancellationToken cancellationToken);
    }
}
