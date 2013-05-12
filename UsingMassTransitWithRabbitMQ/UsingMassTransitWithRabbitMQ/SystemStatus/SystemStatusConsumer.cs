using MassTransit;
using Samples.MassTransit.Messages;
using log4net;

namespace Samples.MassTransit.SystemStatus
{
  public class SystemStatusConsumer : Consumes<ISystemStatusMessage>.Context
  {
    private readonly ILog m_Log = LogManager.GetLogger( typeof( SystemStatusConsumer ) );

    public void Consume( IConsumeContext<ISystemStatusMessage> context )
    {
      var message = context.Message;
      m_Log.InfoFormat( "Message Received: MemoryLoad {0}, CpuLoad {1}", message.MemoryLoad, message.CpuLoad );
    }
  }
}