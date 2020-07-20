using System;
using System.Collections.Generic;

namespace Tom.Lib.Observers
{
    public class DefaultObservable<TMessage> : IObservable<TMessage>
    {
        public List<IObserver<TMessage>> observers = new List<IObserver<TMessage>>();

        public IDisposable Subscribe(IObserver<TMessage> observer)
        {
            observers.Add(observer);

            return new UnSubscriber(observers, observer);
        }

        private class UnSubscriber : IDisposable
        {
            public List<IObserver<TMessage>> observers;
            public IObserver<TMessage> observer;

            public UnSubscriber(List<IObserver<TMessage>> observers, IObserver<TMessage> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if(observers!=null && observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
        }
    }
}
