using System;
using System.Windows.Input;

namespace MusicPlayer
{
    public delegate void ClockUpdatedEventHandler(object sender, ClockUpdatedEventArgs e);
    public delegate void ClockCompletedEventHandler(object sender, EventArgs e);

    public delegate void MediaOpenedEventHandler(object sender, MediaOpenedEventArgs e);
    public delegate void MediaEndedEventHandler(object sender, EventArgs e);
    public delegate void MediaErrorEventHandler(object sender, EventArgs e);

    public delegate void FileChangedEventHandler(object sender, System.IO.FileSystemEventArgs e);
    public delegate void FileRenamedEventHandler(object sender, System.IO.RenamedEventArgs e);
}
