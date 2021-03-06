﻿using OpenTl.Common.IoC;
using OpenTl.Common.Testing;

namespace OpenTl.ClientApi.MtProto.FunctionalTests.Framework
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using DotNetty.Common;

    using log4net;

    using Microsoft.Extensions.Configuration;

    using Moq;

    using OpenTl.ClientApi.MtProto.FunctionalTests.Settings;

    using Xunit.Abstractions;

    public abstract class FunctionalTest : BaseTest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FunctionalTest));

        public override IWindsorContainer Container { get; } = WindsorFactory.Create(typeof(INettyBootstrapper).GetTypeInfo().Assembly);

        private readonly LogOutputTester _logOutputTester;

        private static readonly IConfigurationRoot Configuration;

        protected IRequestSender RequestSender { get; }
        
        static FunctionalTest()
        {
            Configuration = new ConfigurationBuilder()
                                 .AddJsonFile("appsettings.json")
                                 .AddJsonFile("appsettings.debug.json", true)
                                 .Build();
        }

        protected FunctionalTest(ITestOutputHelper output)
        {
            ResourceLeakDetector.Level = ResourceLeakDetector.DetectionLevel.Advanced;

            _logOutputTester = new LogOutputTester(output);

            PrepareToTesting();

            Container.Resolve<INettyBootstrapper>().Init();

            RequestSender = Container.Resolve<IRequestSender>();
        }


        private void PrepareToTesting()
        {
            Container.Register(Component.For<ISessionWriter>().ImplementedBy<SessionWriter>().LifestyleSingleton());

            var settings = new TestSettings();
            Container.Register(Component.For<IClientSettings>().Instance(settings));
            
            settings.AppHash = Configuration[nameof(settings.AppHash)];
            
            settings.AppId = Convert.ToInt32(Configuration[nameof(settings.AppId)]);
            
            settings.PublicKey = Configuration[nameof(settings.PublicKey)];
            
            settings.ClientSession.Port = Convert.ToInt32(Configuration[nameof(settings.ClientSession.Port)]);
            
            settings.ClientSession.ServerAddress = Configuration[nameof(settings.ClientSession.ServerAddress)];

            Log.Info(
                $"\n\n#################################################  {DateTime.Now}  ################################################################################\n\n");
        }
    }
}