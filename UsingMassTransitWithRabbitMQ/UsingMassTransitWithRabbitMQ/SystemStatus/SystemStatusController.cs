using System;
using System.Diagnostics;
using MassTransit;

namespace Samples.MassTransit.SystemStatus
{
  public class SystemStatusController
  {
    private readonly IServiceBus m_Bus;
    private readonly PerformanceCounter m_CpuPerformanceCounter;
    private readonly PerformanceCounter m_RamPerformanceCounter;

    public SystemStatusController( IServiceBus bus, PerformanceCounter cpuPerformanceCounter, PerformanceCounter ramPerformanceCounter )
    {
      if( bus == null )
      {
        throw new ArgumentNullException( "bus" );
      }
      if( cpuPerformanceCounter == null )
      {
        throw new ArgumentNullException( "cpuPerformanceCounter" );
      }
      if( ramPerformanceCounter == null )
      {
        throw new ArgumentNullException( "ramPerformanceCounter" );
      }
      m_Bus = bus;
      m_CpuPerformanceCounter = cpuPerformanceCounter;
      m_RamPerformanceCounter = ramPerformanceCounter;
    }

    public void PublishSystemStatus()
    {
      var message = new SystemStatusMessage( (int) m_CpuPerformanceCounter.NextValue(), (int) m_RamPerformanceCounter.NextValue() );
      m_Bus.Publish( message );
    }
  }
}