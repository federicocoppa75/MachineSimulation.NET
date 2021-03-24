using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.Data.Enums
{
    public enum ToolHolderType
    {
        Static,         // cambio utensile manuale
        AutoSource,     // porta utensile (posizione magazzino)
        AutoSink        // mandrino con cambio utensile automatico
    }
}
