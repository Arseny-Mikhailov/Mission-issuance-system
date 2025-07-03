using System;

public interface IMission
{
    event Action OnStarted;
    event Action OnFinished;
    event Action OnMissionPointReached;

    void Begin();
}