using MassTransit;
using Samples.MassTransit.Messages;
using log4net;

namespace Samples.MassTransit.SystemStatus
{
  public class SystemStatusConsumer : Consumes<ISystemStatusMessage>.All
  {
    private readonly ILog m_Log = LogManager.GetLogger( typeof( SystemStatusConsumer ) );

    public void Consume( ISystemStatusMessage message )
    {
      m_Log.InfoFormat( "Message Received: MemoryLoad {0}, CpuLoad {1}", message.MemoryLoad, message.CpuLoad );
    }
  }
}