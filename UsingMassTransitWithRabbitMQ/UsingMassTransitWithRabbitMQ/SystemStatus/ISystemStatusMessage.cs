using System;
using MassTransit;

namespace Samples.MassTransit.Messages
{
  public interface ISystemStatusMessage : CorrelatedBy<Guid>
  {
    int CpuLoad
    {
      get;
    }

    int MemoryLoad
    {
      get;
    }
  }
}