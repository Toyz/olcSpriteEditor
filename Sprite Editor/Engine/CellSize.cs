using System;
using System.Configuration;

namespace SPE
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class CellSize
    {
        public CellSize(string title, int size)
        {
            Title = title;
            Size = size;
        }

        public CellSize() { }

        public string Title { get; set; }
        public int Size { get; set; }
    }
}