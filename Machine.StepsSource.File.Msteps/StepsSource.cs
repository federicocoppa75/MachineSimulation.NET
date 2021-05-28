using Machine.Steps.ViewModels;
using Machine.Steps.ViewModels.Interfaces;
using Machine.ViewModels.Base;
using Machine.ViewModels.UI;
using MachineSteps.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MVMIoc = Machine.ViewModels.Ioc;

namespace Machine.StepsSource.File.Msteps
{
    public class StepsSource : IStepsSource
    {
        public string Name => "File.msteps";

        private ICommand _loadStepsCommand;
        public ICommand LoadStepsCommand { get { return _loadStepsCommand ?? (_loadStepsCommand = new RelayCommand(() => LoadStepsCommandImplementation())); } }

        private void LoadStepsCommandImplementation()
        {
            var dlg = MVMIoc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");
            var data = MVMIoc.SimpleIoc<IStepsContainer>.GetInstance();

            dlg.AddExtension = true;
            dlg.DefaultExt = "msteps";
            dlg.Filter = "Machine step |*.msteps";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var doc = (MachineStepsDocument)serializer.Deserialize(reader);

                    if (doc != null)
                    {
                        data.Steps.Clear();

                        for (int i = 0; i < doc.Steps.Count; i++)
                        {
                            data.Steps.Add(new StepViewModel(doc.Steps[i], i + 1));
                        }

                        data.SourceName = dlg.FileName;
                    }
                }
            }

        }
    }
}
