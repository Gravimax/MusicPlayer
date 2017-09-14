namespace MusicPlayer
{
    public class GetNextFileMessage
    {
        public GetNextFileMessage(PlayType playType)
        {
            SelectedPlayType = playType;
        }

        public readonly PlayType SelectedPlayType;
    }
}
