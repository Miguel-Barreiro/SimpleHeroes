namespace Gram.Core
{
    public static class GameBasics
    {
        public delegate void SimpleDelegate();

        public delegate void SingleParameterDelegate<T>(T arg);
    }
}