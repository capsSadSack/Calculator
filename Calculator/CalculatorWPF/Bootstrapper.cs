using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using CalculatorWPF.ViewModels;

namespace CalculatorWPF
{
    internal static class Bootstrapper
    {
        private static ILifetimeScope _rootScope;
        private static IMainViewModel _mainViewModel;

        public static IViewModel RootVisual
        {
            get
            {
                if (_rootScope == null)
                {
                    Start();
                }

                _mainViewModel = _rootScope.Resolve<IMainViewModel>();
                return _mainViewModel;
            }
        }

        public static void Start()
        {
            if (_rootScope != null)
            {
                return;
            }

            var builder = new ContainerBuilder();
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            builder.RegisterType<MainViewModel>()
                .As<IMainViewModel>()
                .SingleInstance();
                

            _rootScope = builder.Build();
        }

        public static void Stop()
        {
            _rootScope.Dispose();
        }

        public static T Resolve<T>()
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(new Parameter[0]);
        }

        public static T Resolve<T>(Parameter[] parameters)
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(parameters);
        }
    }
}
