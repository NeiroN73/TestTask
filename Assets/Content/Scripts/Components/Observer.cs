namespace Game.Components
{ 
    using R3;
    using System;

    public abstract class ObserverBase : IDisposable
    {
        protected readonly CompositeDisposable _compositeDisposable = new();

        public void Unsubscribe(IDisposable disposable)
        {
            disposable?.Dispose();
        }
        
        public void UnsubscribeAll()
        {
            _compositeDisposable.Clear();
        }

        public virtual void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        protected IDisposable AddToDisposable(IDisposable disposable)
        {
            return disposable.AddTo(_compositeDisposable);
        }
    }

    public class Observer : ObserverBase
    {
        private readonly Subject<Unit> _subject = new();

        public void Publish()
        {
            _subject.OnNext(Unit.Default);
        }

        public IDisposable Subscribe(Action callback)
        {
            return AddToDisposable(_subject.Subscribe(_ => callback?.Invoke()));
        }

        public Observable<Unit> AsObservable() => _subject;

        public override void Dispose()
        {
            _subject.Dispose();
            base.Dispose();
        }
    }

    public class Observer<T> : ObserverBase
    {
        private readonly Subject<T> _subject = new();

        public void Publish(T value)
        {
            _subject.OnNext(value);
        }

        public IDisposable Subscribe(Action<T> callback)
        {
            return AddToDisposable(_subject.Subscribe(callback));
        }

        public Observable<T> AsObservable() => _subject;

        public override void Dispose()
        {
            _subject.Dispose();
            base.Dispose();
        }
    }

    public class Observer<T1, T2> : ObserverBase
    {
        private readonly Subject<(T1, T2)> _subject = new();

        public void Publish(T1 value1, T2 value2)
        {
            _subject.OnNext((value1, value2));
        }

        public IDisposable Subscribe(Action<T1, T2> callback)
        {
            return AddToDisposable(_subject.Subscribe(tuple => callback(tuple.Item1, tuple.Item2)));
        }

        public Observable<(T1, T2)> AsObservable() => _subject;

        public override void Dispose()
        {
            _subject.Dispose();
            base.Dispose();
        }
    }

    public class Observer<T1, T2, T3> : ObserverBase
    {
        private readonly Subject<(T1, T2, T3)> _subject = new();

        public void Publish(T1 value1, T2 value2, T3 value3)
        {
            _subject.OnNext((value1, value2, value3));
        }

        public IDisposable Subscribe(Action<T1, T2, T3> callback)
        {
            return AddToDisposable(_subject.Subscribe(tuple => callback(tuple.Item1, tuple.Item2, tuple.Item3)));
        }

        public Observable<(T1, T2, T3)> AsObservable() => _subject;

        public override void Dispose()
        {
            _subject.Dispose();
            base.Dispose();
        }
    }

    public class Observer<T1, T2, T3, T4> : ObserverBase
    {
        private readonly Subject<(T1, T2, T3, T4)> _subject = new();

        public void Publish(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            _subject.OnNext((value1, value2, value3, value4));
        }

        public IDisposable Subscribe(Action<T1, T2, T3, T4> callback)
        {
            return AddToDisposable(_subject.Subscribe(tuple => callback(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4)));
        }

        public Observable<(T1, T2, T3, T4)> AsObservable() => _subject;

        public override void Dispose()
        {
            _subject.Dispose();
            base.Dispose();
        }
    }

    public class Observer<T1, T2, T3, T4, T5> : ObserverBase
    {
        private readonly Subject<(T1, T2, T3, T4, T5)> _subject = new();

        public void Publish(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            _subject.OnNext((value1, value2, value3, value4, value5));
        }

        public IDisposable Subscribe(Action<T1, T2, T3, T4, T5> callback)
        {
            return AddToDisposable(_subject.Subscribe(tuple => callback(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5)));
        }

        public Observable<(T1, T2, T3, T4, T5)> AsObservable() => _subject;

        public override void Dispose()
        {
            _subject.Dispose();
            base.Dispose();
        }
    }
}