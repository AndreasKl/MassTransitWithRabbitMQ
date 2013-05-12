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

      var bus = ServiceBusFactory.New(
        sbc =>
        {
          sbc.UseLog4Net();
          sbc.UseRabbitMq();
          sbc.ReceiveFrom( "rabbitmq://localhost/sample" );
          sbc.SetShutdownTimeout( TimeSpan.FromSeconds( 5 ) );
          sbc.Subscribe( x => x.LoadFrom( container ) );
        } );

      var ramCounter = new PerformanceCounter( "Memory", "Available MBytes" );
      var cpuCounter = new PerformanceCounter( "Process", "% Processor Time", "_Total" );

      container.Register( Component.For<IServiceBus>().Instance( bus ) );
      container.Register(
        Component.For<SystemStatusController>()
                 .ImplementedBy<SystemStatusController>()
                 .DependsOn(
                   Dependency.OnComponent( "ramPerformanceCounter", "Memory" ),
                   Dependency.OnComponent( "cpuPerformanceCounter", "Processor" )
          ) );
      container.Register( Component.For<PerformanceCounter>().Instance( cpuCounter ).Named( "Processor" ) );
      container.Register( Component.For<PerformanceCounter>().Instance( ramCounter ).Named( "Memory" ) );
    }
  }
}