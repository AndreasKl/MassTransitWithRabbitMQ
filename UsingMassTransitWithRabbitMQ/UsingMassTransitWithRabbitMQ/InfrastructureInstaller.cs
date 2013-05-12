using System;
using System.Diagnostics;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Log4NetIntegration;
using Samples.MassTransit.SystemStatus;

namespace Samples.MassTransit
{
  public class InfrastructureInstaller : IWindsorInstaller
  {
    public void Install( IWindsorContainer container, IConfigurationStore store )
    {
      container.Register( AllTypes.FromThisAssembly().BasedOn<IConsumer>() );

      Func<PerformanceCounter> processorCounterFactoryMethod = () => new PerformanceCounter( "Process", "% Processor Time", "_Total" );
      container.Register( Component.For<PerformanceCounter>().UsingFactoryMethod( processorCounterFactoryMethod ).Named( "Processor" ) );

      Func<PerformanceCounter> memoryCounterFactoryMethod = () => new PerformanceCounter( "Memory", "Available MBytes" );
      container.Register( Component.For<PerformanceCounter>().UsingFactoryMethod( memoryCounterFactoryMethod ).Named( "Memory" ) );

      Func<IServiceBus> serviceBusFactoryMethod = () => ServiceBusFactory.New(
        sbc =>
        {
          sbc.UseLog4Net();
          sbc.UseRabbitMq();
          sbc.ReceiveFrom( "rabbitmq://localhost/sample" );
          sbc.Subscribe( x => x.LoadFrom( container ) );
        } );
      container.Register( Component.For<IServiceBus>().UsingFactoryMethod( serviceBusFactoryMethod ) );

      container.Register(
        Component.For<SystemStatusController>()
                 .ImplementedBy<SystemStatusController>()
                 .DependsOn(
                   Dependency.OnComponent( "ramPerformanceCounter", "Memory" ),
                   Dependency.OnComponent( "cpuPerformanceCounter", "Processor" )
          ) );
    }
  }
}