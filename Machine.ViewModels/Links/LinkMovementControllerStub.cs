using Machine.ViewModels.Interfaces.Links;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.Links
{
    public class LinkMovementControllerStub : ILinkMovementController
    {
        public int MinTimespam { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Enable { get => false; set => throw new NotImplementedException(); }
        public double TimespanFactor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
