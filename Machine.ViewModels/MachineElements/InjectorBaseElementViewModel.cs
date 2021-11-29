using Machine.Data.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Machine.ViewModels.MachineElements
{
    public abstract class InjectorBaseElementViewModel : ElementViewModel
    {
        private int _inserterId;
        public int InserterId 
        { 
            get => _inserterId; 
            set => Set(ref _inserterId, value, nameof(InserterId)); 
        }

        private Point _position;
        public Point Position 
        { 
            get => _position; 
            set => Set(ref _position, value, nameof(Position)); 
        }

        private Vector _direction;
        public Vector Direction 
        { 
            get => _direction; 
            set => Set(ref _direction, value, nameof(Direction)); 
        }

        private Color _inserterColor;
        public Color InserterColor 
        { 
            get => _inserterColor; 
            set => Set(ref _inserterColor, value, nameof(InserterColor)); 
        }
    }
}
