namespace SugarSqlCore
{
    public class CurrentUserAmbient
    {
        private static readonly AsyncLocal<Guid?> UserIdHolder = new();

        public static Guid? UserId => Get();

        public static Guid? Get()
        {
            return UserIdHolder.Value;
        }

        public static void Set(Guid? userId)
        {
            UserIdHolder.Value = userId;
        }

        public static void Clear()
        {
            UserIdHolder.Value = null;
        }
    }
}
