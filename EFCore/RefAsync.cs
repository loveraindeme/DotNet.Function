namespace EFCore
{
    public class RefAsync<T>
    {
        public T Value { get; set; }

        public RefAsync()
        {
            Value = default!;
        }

        public RefAsync(T value)
        {
            Value = value;
        }

        public static implicit operator T(RefAsync<T> refAsync) => refAsync.Value;
    }
}
