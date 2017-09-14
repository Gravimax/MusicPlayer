namespace MusicPlayer
{
    public class SelectionChangedMessage
    {
        public SelectionChangedMessage(int index)
        {
            Index = index;
        }

        public readonly int Index;
    }
}
