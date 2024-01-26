namespace StdNounou.UI
{
    public interface IHidableUI
    {
        public void OpenCloseFlipFlop(bool playTween);
        public void Open(bool playTween);
        public void Close(bool playTween);
    } 
}