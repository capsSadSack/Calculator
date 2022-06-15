using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using CalculatorWPF.EventAggregation;
using CalculatorWPF.ViewModels;
using CalculatorWPF.Views;

namespace CalculatorWPF
{
    internal static class Bootstrapper
    {
        public static ILifetimeScope RootScope { get; private set; }

        private static IMainViewModel _mainViewModel;

        public static IViewModel RootVisual
        {
            get
            {
                if (RootScope == null)
                {
                    Start();
                }

                _mainViewModel = RootScope.Resolve<IMainViewModel>();
                return _mainViewModel;
            }
        }

        public static void Start()
        {
            if (RootScope != null)
            {
                return;
            }

            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>()
                .As<IEventAggregator>()
                .SingleInstance();

            builder.RegisterType<MainViewModel>()
                .As<IMainViewModel>()
                .SingleInstance();

            builder.RegisterType<MainView>()
                .SingleInstance();

            builder.RegisterType<KeyboardViewModel>()
                .SingleInstance();

            builder.RegisterType<DialViewModel>()
                .SingleInstance();

            RootScope = builder.Build();
        }

        public static void Stop()
        {
            RootScope.Dispose();
        }

        public static T Resolve<T>()
        {
            if (RootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return RootScope.Resolve<T>();
        }
    }
}
