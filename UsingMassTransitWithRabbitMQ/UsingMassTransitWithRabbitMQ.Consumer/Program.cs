using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MassTransit;
using log4net;

namespace UsingMassTransitWithRabbitMQ.Consumer
{
  public class Program
  {
    private static readonly ILog Log = LogManager.GetLogger( typeof( Program ) );

    public static void Main( string[] args )
    {
      Log.Info( "Starting mass transit consumer." );

      var container = new WindsorContainer();
      container.Install( FromAssembly.This() );
      
      // Resolve the bus to start the consumer
      container.Resolve<IServiceBus>();

      Console.WriteLine( "Press any key to exit." );
      Console.ReadKey();

      container.Dispose();
    }
  }
}