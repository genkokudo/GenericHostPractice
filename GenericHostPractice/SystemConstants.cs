namespace GenericHostPractice
{
    class SystemConstants
    {
        /// <summary>
        /// 環境を示す環境変数名のプレフィクス
        /// </summary>
        public const string PrefixEnv = "DOTNETCORE_";
        /// <summary>
        /// DBパスワード環境変数名
        /// </summary>
        public const string DbPasswordEnv = "DATABASE_PASSWORD";
        /// <summary>
        /// 環境を示す環境変数名のプレフィクス
        /// </summary>
        public const string EnxEnv = PrefixEnv + "ENVIRONMENT";

        /// <summary>
        /// 現在のDB接続設定
        /// 詳細はappsettings.json
        /// </summary>
        public const string Connection = "DefaultConnection";

        // 環境
        /// <summary>
        /// 環境：開発系
        /// </summary>
        public const string EnvDevelopment = "Development";
        /// <summary>
        /// 環境：本番系
        /// </summary>
        public const string EnvProduction = "Production";

    }
}
