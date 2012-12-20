using System;
using Retlang.Core;

namespace Retlang.Channels
{
    /// <summary>
    /// Base implementation for subscription
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseSubscription<T> : ISubscribable<T>
    {
        private Predicate<T> _filterOnProducerThread;

        /// <summary>
        /// <see cref="ISubscribable{T}.FilterOnProducerThread"/>
        /// </summary>
        public Predicate<T> FilterOnProducerThread
        {
            get { return _filterOnProducerThread; }
            set { _filterOnProducerThread = value; }
        }

        private bool PassesProducerThreadFilter(T msg)
        {
            return _filterOnProducerThread == null || _filterOnProducerThread(msg);
        }

        /// <summary>
        /// <see cref="IProducerThreadReceiver{T}.ReceiveOnProducerThread"/>
        /// </summary>
        /// <param name="msg"></param>
        public void ReceiveOnProducerThread(T msg)
        {
            if (PassesProducerThreadFilter(msg))
            {
                OnMessageOnProducerThread(msg);
            }
        }

        ///<summary>
        /// Allows for the registration and deregistration of subscriptions
        ///</summary>
        public abstract ISubscriptionRegistry Subscriptions { get; }

        /// <summary>
        /// Called after message has been filtered.
        /// </summary>
        /// <param name="msg"></param>
        protected abstract void OnMessageOnProducerThread(T msg);
    }
}
