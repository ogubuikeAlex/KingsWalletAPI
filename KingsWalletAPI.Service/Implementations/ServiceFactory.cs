using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingsWalletAPI.Service.Interfaces;

namespace KingsWalletAPI.Service.Implementations
{
    public class ServiceFactory : IServiceFactory
    {

        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetServices<T>() where T : class
        {
            var newservice = _serviceProvider.GetService(typeof(T));
            return (T)newservice;
        }
    }
}
