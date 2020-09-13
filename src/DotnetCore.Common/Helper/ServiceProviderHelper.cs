using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetCore.Common.Helper
{
    /// <summary>
    /// IServiceProvider 静态访问类
    /// </summary>
    public static class ServiceProviderHelper
    {
        /// <summary>
        /// 获取当前IServiceProvider对象
        /// </summary>
        public static IServiceProvider Current { get; private set; }

        internal static void Configure(IServiceProvider serviceProvider)
        {
            Current = serviceProvider;
        }
    }
}
