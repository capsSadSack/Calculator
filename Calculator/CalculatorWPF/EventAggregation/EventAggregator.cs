using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorWPF.EventAggregation
{

    public class EventAggregator : IEventAggregator, IDisposable
    {
        private Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();


        public EventAggregator()
        {
        }


        public void Dispose()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
            }
        }

        public async Task NotifyAsync<T>(T message)
        {
            Type messageType = typeof(T);

            if(_subscribers.Keys.Contains(messageType))
            {
                var subscribers = _subscribers[messageType];

                List<Task> tasks = new List<Task>();

                foreach (var subscriber in subscribers)
                {
                    var sub = (IHandle<T>)subscriber;
                    await sub.HandleAsync(message, _tokenSource.Token);
                }
            }
        }

        public void Subscribe<T>(IHandle<T> subscriber)
        {
            Type messageType = typeof(T);

            if(!_subscribers.Keys.Contains(messageType))
            {
                _subscribers.Add(messageType, new List<object>());
            }

            _subscribers[messageType].Add(subscriber);
        }

        public void Unsubscribe<T>(IHandle<T> subscriber)
        {
            Type messageType = typeof(T);

            if (_subscribers.Keys.Contains(messageType))
            {
                _subscribers[messageType].Remove(subscriber);
            }
        }
    }
}
