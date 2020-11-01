using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLabo.Infrastructure.Settings
{

    /// <summary>
    /// appsettings.jsonから値を読み込むためのクラス
    /// </summary>
    public class DefaultParameters
    {
        public bool IsDemoMode { get; set; }
        public DefaultUser DefaultUser { get; set; }
    }

    public class DefaultUser
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
