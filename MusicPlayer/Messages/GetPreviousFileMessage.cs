namespace MusicPlayer
{
    public class GetPreviousFileMessage
    {
        public GetPreviousFileMessage(PlayType playType)
        {
            SelectedPlayType = playType;
        }

        public readonly PlayType SelectedPlayType;
    }
}
