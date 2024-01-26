namespace StdNounou.UI
{
    public interface IEffectPlayer
    {
        public enum E_StartAndStopBehaviour
        {
            SetAtStart,
            SetAtEnd,
            Stay
        }

        public enum E_EffectState
        {
            Playing,
            Paused,
            Stopped,
        }

        public enum E_LoopType
        {
            NoLoop,
            Loop,
            PingPong,
        }

        public abstract bool TryPlayForward(E_StartAndStopBehaviour startBehaviour = E_StartAndStopBehaviour.Stay, bool stopIfPlaying = false);
        public abstract void PlayForward();
        public abstract void Pause();
        public abstract void Resume();
        public abstract void Stop(E_StartAndStopBehaviour afterStopState, bool breakLoop, bool callDelegate = true);

        public abstract bool IsPlaying();
        public abstract bool IsPaused();

        public abstract bool TryPlayReverse(E_StartAndStopBehaviour startBehaviour = E_StartAndStopBehaviour.Stay, bool stopIfPlaying = false);
        public abstract void PlayReverse();

        public abstract void SetPlaybackSpeed(float speed);

        public abstract void SetLoopType(E_LoopType loopType, int count = -1);
        public abstract void SetLoopCount(int count);

        public E_EffectState GetEffectState();
    } 
}
