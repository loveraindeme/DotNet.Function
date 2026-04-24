namespace SugarSqlCore
{
    public class CurrentUserAmbient : CurrentUserAmbient<Guid>
    {

    }

    public class CurrentUserAmbient<TKey>
    {
        private static readonly AsyncLocal<TKey?> UserIdHolder = new();

        public static TKey? UserId => Get();

        public static TKey? Get()
        {
            return UserIdHolder.Value;
        }

        public static void Set(TKey? userId)
        {
            UserIdHolder.Value = userId;
        }

        public static void Clear()
        {
            UserIdHolder.Value = default;
        }
    }
}
