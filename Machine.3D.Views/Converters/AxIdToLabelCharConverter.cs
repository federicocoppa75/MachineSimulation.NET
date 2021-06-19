using Machine.ViewModels;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Machine._3D.Views.Converters
{
    class AxIdToLabelCharConverter : IValueConverter
    {
        IKernelViewModel _kernel;
        private IKernelViewModel Kernel => _kernel ?? (_kernel = Machine.ViewModels.Ioc.SimpleIoc<IKernelViewModel>.GetInstance());

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertImplementation(value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object ConvertImplementation(object value)
        {
            if (value is int id)
            {
                if((Kernel != null) && 
                    (Kernel.Machines.Count > 0) && 
                    (Kernel.Machines[0] is IRootElement re) && 
                    (re.RootType == Data.Enums.RootType.CX220))
                {
                    return ConvertCx220Implementation(id);
                }
                else
                {
                    return ConvertDefaultImplementation(id);
                }
            }
            else
            {
                return value;
            }
        }

        private object ConvertCx220Implementation(int id)
        {
            switch (id)
            {
                case 1:     return "X";
                case 2:     return "U";
                case 101:   return "Y";
                case 201:   return "V";
                case 102:   return "Z";
                case 202:   return "W";
                case 112:   return "A";
                case 212:   return "B";
                default:    return id;
            }
        }

        private object ConvertDefaultImplementation(int id)
        {
            switch (id)
            {
                case 1: return "X";
                case 2: return "Y";
                case 3: return "Z";
                case 4: return "U";
                case 5: return "V";
                case 6: return "W";
                case 7: return "A";
                case 8: return "B";
                case 0: return "C";
                default: return id;
            }
        }
    }
}
