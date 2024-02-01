using Godot;

namespace shared;

public static class TweenExtensions
{
    public static SignalAwaiter GetFinishedAwaiter(this Tween tween)
    {
        return tween.ToSignal(tween, Tween.SignalName.Finished);
    }
}