using System.ComponentModel;

namespace MusicPlayer
{
    public class PlayListConfigUpdateMessage
    {
        public PlayListConfigUpdateMessage(string sortColumn, ListSortDirection sortDirection)
        {
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }

        public readonly string SortColumn;

        public readonly ListSortDirection SortDirection;
    }
}
