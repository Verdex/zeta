
namespace Zeta.Util
{
    public static class Option
    {
        public static Option<T> Some<T>( T value ) 
        {
            return new Some<T>( value );
        }

        public static Option<T> Nothing<T>()
        {
            return new Nothing<T>();
        }
    }

    public interface Option<T>
    {
    }
    
    public struct Some<T> : Option<T>
    {
        public Some( T value ) 
        {
            Value = value;
        }
        public T Value { get; }
    }

    public struct Nothing<T> : Option<T> { }
}

