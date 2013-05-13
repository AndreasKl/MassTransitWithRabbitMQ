namespace Samples.MassTransit.Messages
{
  public interface ISystemStatusMessage
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