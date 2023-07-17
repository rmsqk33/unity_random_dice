
using System.Diagnostics;

public class FTimer
{
    private float interval;

    public float Interval { get { return interval; } set { interval = value; } }
    Stopwatch stopWatch = new Stopwatch();

    public int Hours { get { return (int)(Elapsed / 3600) % 24; } }
    public int Minutes { get { return (int)(Elapsed / 60) % 60; } }
    public int Seconds { get { return (int)(Elapsed) % 60; } }
    public int Elapsed { get { return (int)stopWatch.ElapsedMilliseconds / 1000; } }
    
    public FTimer()
    {
    }

    public FTimer(float InInterval)
    {
        interval = InInterval;
    }

    public void Start()
    {
        stopWatch.Start();
    }

    public void Restart()
    {
        stopWatch.Restart();
    }

    public void Stop()
    {
        stopWatch.Stop();
    }

    public bool IsElapsedCheckTime()
    {
        if (stopWatch.IsRunning == false)
            return false;

        return interval <= stopWatch.ElapsedMilliseconds / 1000.0f;
    }

    public string ToString(string InFormat)
    {
        string retVal = InFormat;

        retVal = retVal.Replace("h", Hours.ToString());
        retVal = retVal.Replace("m", Minutes.ToString());
        retVal = retVal.Replace("s", Seconds.ToString());

        return retVal;
    }
}
