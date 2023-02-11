using Machine.Data.Base;
using Machine.ViewModels.Interfaces.Indicators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public abstract class InjectorBaseElementViewModel : ElementViewModel, IPositionAndDirectionIndicator
    {
        private int _inserterId;
        public int InserterId 
        { 
            get => _inserterId; 
            set => Set(ref _inserterId, value, nameof(InserterId)); 
        }

        private Point _position = new Point();
        public Point Position 
        { 
            get => _position; 
            set => Set(ref _position, value, nameof(Position)); 
        }

        private Vector _direction = new Vector() { Z = -1 };
        public Vector Direction 
        { 
            get => _direction; 
            set => Set(ref _direction, value, nameof(Direction)); 
        }

        private Color _inserterColor = new Color() { R = 255, A = 255};
        public Color InserterColor 
        { 
            get => _inserterColor; 
            set => Set(ref _inserterColor, value, nameof(InserterColor)); 
        }
    }
}
