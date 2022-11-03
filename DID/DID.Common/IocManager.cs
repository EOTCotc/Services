using Autofac;

namespace DID.Common
{
    public class IocManager
    {
        private static object obj = new object();
        private static ILifetimeScope _container { get; set; }

        public static void InitContainer(ILifetimeScope container)
        {
            //防止过程中方法被调用_container发生改变
            if (_container == null)
            {
                lock (obj)
                {
                    if (_container == null)
                    {
                        _container = container;
                    }
                }
            }
        }
        /// <summary>
        /// 手动获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
