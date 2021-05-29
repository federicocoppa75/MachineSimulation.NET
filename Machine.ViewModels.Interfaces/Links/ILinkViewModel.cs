using Machine.Data.Enums;
using System;

namespace Machine.ViewModels.Interfaces.Links
{
    public interface ILinkViewModel
    {
        string Description { get; set; }
        LinkDirection Direction { get; set; }
        int Id { get; set; }
        LinkMoveType MoveType { get; }
        LinkType Type { get; set; }
        double Value { get; set; }

        event EventHandler<double> ValueChanged;
    }
}