using Samples.MassTransit.Messages;

namespace Samples.MassTransit.SystemStatus
{
  public class SystemStatusMessage : ISystemStatusMessage
  {
    public SystemStatusMessage( int cpuLoad, int memoryLoad )
    {
      CpuLoad = cpuLoad;
      MemoryLoad = memoryLoad;
    }

    public int CpuLoad
    {
      get;
      private set;
    }

    public int MemoryLoad
    {
      get;
      private set;
    }
  }
}