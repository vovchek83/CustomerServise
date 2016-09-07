using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Windows;
using Client.Navigation.Messages;
using CustomersService.DataModel;
using CustomersService.DataModel.Repositories;
using CustomersService.Infrastructure;

namespace Client
{
    public class Bootstrapper : BootstrapperBase
    {

        private CompositionContainer _container;

        private static bool _logFileDeleted;

        public Bootstrapper()
        {

        }

        protected override void Configure()
        {

            //  LogManager.GetLog = type => new FileLogger(type, CanDeleteLogFile);
            LogManager.GetLog = type => new Logger();


            _container = new CompositionContainer(
                new AggregateCatalog(
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
                    )
                );

            var batch = new CompositionBatch();


            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());

            batch.AddExportedValue(_container);

            _container.Compose(batch);

            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());

            new CustomersServiceDbContext().Database.Initialize(true);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            ILoginConductor loginConductor = _container.GetExportedValue<ILoginConductor>();
            IEventAggregator events = _container.GetExportedValue<IEventAggregator>();

            events.BeginPublishOnUIThread(new LoginMessage());

        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetExecutingAssembly(), typeof(UsersRepository).Assembly };
        }

        private static bool CanDeleteLogFile()
        {
            bool output;
            if (!_logFileDeleted)
            {
                output = true;
                _logFileDeleted = true;
            }
            else
            {
                output = false;
            }

            return output;
        }
    }
}
