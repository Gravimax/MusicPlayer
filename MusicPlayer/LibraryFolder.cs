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
    
    public partial class LibraryFolder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int LibraryId { get; set; }
        public bool IsExpanded { get; set; }
        public string ParentFolder { get; set; }
    
        public virtual Library Library { get; set; }
    }
}
