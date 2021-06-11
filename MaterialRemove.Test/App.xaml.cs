using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MVMIoc = Machine.ViewModels.Ioc;
using MRVM3D = MaterialRemove.ViewModels._3D;
using MRVMI = MaterialRemove.ViewModels.Interfaces;

namespace MaterialRemove.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            MVMIoc.SimpleIoc<MRVMI.IElementViewModelFactory>.Register<MRVM3D.ElementViewModelFactory>();
        }      
    }
}
