namespace MusicPlayer
{
    public class ListTypeUpdatedMessage
    {
        public ListTypeUpdatedMessage(ListType listType, int currPlaylistId)
        {
            this.ListType = listType;
        }

        public readonly ListType ListType;
        public readonly int CurrentPlaylistId;
    }
}
