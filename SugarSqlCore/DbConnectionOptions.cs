namespace SugarSqlCore
{
    public class DbConnectionOptions
    {
        public const string DbConnectionOption = "DbConnection";

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
