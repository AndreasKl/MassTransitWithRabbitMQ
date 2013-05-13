using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Log4NetIntegration;

namespace Samples.MassTransit
{
  public class ProducerInstaller : IWindsorInstaller
  {
    public void Install( IWindsorContainer container, IConfigurationStore store )
    {
      container.Register( AllTypes.FromThisAssembly().BasedOn<IConsumer>() );

      Func<IServiceBus> serviceBusFactoryMethod = () => ServiceBusFactory.New(
        sbc =>
        {
          sbc.UseLog4Net();
          sbc.UseRabbitMq();
          sbc.ReceiveFrom( "rabbitmq://localhost/sample_consumer" );
          sbc.Subscribe( x => x.LoadFrom( container ) );
          sbc.UseControlBus();
          sbc.Validate();
        } );
      container.Register( Component.For<IServiceBus>().UsingFactoryMethod( serviceBusFactoryMethod ) );
    }
  }
}