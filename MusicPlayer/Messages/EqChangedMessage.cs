namespace MusicPlayer
{
    public class EqChangedMessage
    {
        public EqChangedMessage(int filterIndex, float value)
        {
            this.FilterIndex = filterIndex;
            this.Value = value;
        }

        public readonly int FilterIndex;
        public readonly float Value;
    }
}
