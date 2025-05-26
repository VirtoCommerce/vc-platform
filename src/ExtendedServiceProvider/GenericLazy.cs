using Microsoft.Extensions.DependencyInjection;

namespace ExtendedServiceProvider
{
    public class GenericLazy<T> : Lazy<T>
        where T : notnull
    {
        public GenericLazy() { }
        public GenericLazy(bool isThreadSafe) : base(isThreadSafe) { }
        public GenericLazy(Func<T> valueFactory) : base(valueFactory) { }
        public GenericLazy(LazyThreadSafetyMode mode) : base(mode) { }
        public GenericLazy(T value) : base(value) { }
        public GenericLazy(Func<T> valueFactory, bool isThreadSafe) : base(valueFactory, isThreadSafe) { }
        public GenericLazy(Func<T> valueFactory, LazyThreadSafetyMode mode) : base(valueFactory, mode) { }
        public GenericLazy(IServiceProvider serviceProvider) : this(() => serviceProvider.GetRequiredService<T>()) { }

        public static implicit operator T(GenericLazy<T> value) => value.Value;
    }
}
