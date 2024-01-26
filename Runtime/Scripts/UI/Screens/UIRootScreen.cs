namespace StdNounou.UI
{
    public class UIRootScreen : UIScreen
    {
        public override void Open(bool playTween)
        {
            if (IsOpened) return;
            if (playTween) this.RootScreenWillOpen();
            base.Open(playTween);
        }

        public override void Close(bool playTween)
        {
            if (!IsOpened) return;
            if (playTween) this.RootScreenWillClose();
            base.Close(playTween);
        }

        protected override void SequenceEnded()
        {
            base.SequenceEnded();
            if (IsOpened) this.RootScreenOpened();
            else this.RootScreenClosed();
        }
    }
}