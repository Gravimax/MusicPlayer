//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicPlayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class MediaItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MediaItem()
        {
            this.Playlists = new HashSet<Playlist>();
        }
    
        public int Id { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public int BitRate { get; set; }
        public string Comment { get; set; }
        public long Duration { get; set; }
        public string FilePath { get; set; }
        public string Folder { get; set; }
        public string Genre { get; set; }
        public string Title { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Track { get; set; }
        public string Type { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
