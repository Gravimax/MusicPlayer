using System.ComponentModel;

namespace MusicPlayer
{
    public class PlayListConfigSetMessage
    {
        public PlayListConfigSetMessage(string sortColumn, ListSortDirection sortDirection)
        {
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }

        public readonly string SortColumn;

        public readonly ListSortDirection SortDirection;
    }
}
