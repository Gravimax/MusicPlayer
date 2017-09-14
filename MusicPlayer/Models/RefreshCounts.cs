using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Models
{
    public struct RefreshCounts
    {
        public int RemovedCount { get; set; }

        public int NewCount { get; set; }

        public int RemovedFolders { get; set; }

        public int NewFolders { get; set; }

        public RefreshCounts(int removedCount, int newCount, int removedFolders, int newFolders)
        {
            RemovedCount = removedCount;
            NewCount = newCount;
            RemovedFolders = removedFolders;
            NewFolders = newFolders;
        }
    }
}
