using Machine.Steps.ViewModels;
using Machine.Steps.ViewModels.Interfaces;
using Machine.ViewModels.Base;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Messages.Links;
using Machine.ViewModels.Messaging;
using Machine.ViewModels.UI;
using MachineSteps.Models;
using MachineSteps.Plugins.IsoParser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MVMIoc = Machine.ViewModels.Ioc;

namespace Machine.StepsSource.File.Iso
{
    public class StepsSource : IStepsSource
    {
        public string Name => "File.iso";

        private ICommand _loadStepsCommand;
        public ICommand LoadStepsCommand { get { return _loadStepsCommand ?? (_loadStepsCommand = new RelayCommand(() => LoadStepsCommandImplementation())); } }

        private void LoadStepsCommandImplementation()
        {
            var dlg = MVMIoc.SimpleIoc<IFileDialog>.GetInstance("OpenFile");
            var data = MVMIoc.SimpleIoc<IStepsContainer>.GetInstance();

            dlg.AddExtension = true;
            dlg.DefaultExt = "msteps";
            dlg.Filter = "Machine steps |*.msteps|Cnc iso file |*.iso|Cnc iso file |*.i| Cnc iso file |*.i*| Cnc iso file |*.log";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var extension = System.IO.Path.GetExtension(dlg.FileName).Replace(".", "");

                if (string.Compare(extension, "msteps", true) == 0)
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                    using (var reader = new System.IO.StreamReader(dlg.FileName))
                    {
                        var doc = (MachineStepsDocument)serializer.Deserialize(reader);

                        if (doc != null)
                        {
                            LoadDataSteps(data, doc);
                            data.SourceName = dlg.FileName;
                        }
                    }
                }
                else if ((string.Compare(extension, "iso", true) == 0) || 
                    (string.Compare(extension, "i", true) == 0) ||
                    (string.Compare(extension, "log", true) == 0) ||
                    ((extension[0] == 'i') && (int.TryParse(extension.Substring(1), out int i))))
                {
                    var doc = IsoParser.Parse(dlg.FileName, true, GetLinkLimits, GetLinearLinkCount, GetLinearLinksIds);

                    if (doc != null)
                    {
                        LoadDataSteps(data, doc);
                        data.SourceName = dlg.FileName;
                    }
                }
            }


        }

        private static void LoadDataSteps(IStepsContainer data, MachineStepsDocument doc)
        {
            data.Steps.Clear();

            data.Steps.Add(new StepViewModel(-1, "Start", "Initial condition"));

            for (int i = 0; i < doc.Steps.Count; i++)
            {
                data.Steps.Add(new StepViewModel(doc.Steps[i], i + 1));
            }
        }

        private static int GetLinearLinkCount()
        {
            int result = 0;

            MVMIoc.SimpleIoc<IMessenger>.GetInstance().Send(new GetLinkMessage()
            {
                Id = -1,
                SetLink = (link) =>
                {
                    if (link.MoveType == Data.Enums.LinkMoveType.Linear)
                    {
                        result++;
                    }
                }
            });

            return result;
        }

        private static IList<int> GetLinearLinksIds()
        {
            var list = new List<int>();

            MVMIoc.SimpleIoc<IMessenger>.GetInstance().Send(new GetLinkMessage()
            {
                Id = -1,
                SetLink = (link) =>
                {
                    if (link.MoveType == Data.Enums.LinkMoveType.Linear)
                    {
                        list.Add(link.Id);
                    }
                }
            });

            return list;
        }

        private static Tuple<double, double> GetLinkLimits(int id)
        {
            double min = 0.0;
            double max = 0.0;

            MVMIoc.SimpleIoc<IMessenger>.GetInstance().Send(new GetLinkMessage()
            {
                Id = id,
                SetLink = (link) =>
                {
                    min = (link as ILinearLinkViewModel).Min;
                    max = (link as ILinearLinkViewModel).Max;
                }
            });

            return new Tuple<double, double>(min, max);
        }
    }

}
