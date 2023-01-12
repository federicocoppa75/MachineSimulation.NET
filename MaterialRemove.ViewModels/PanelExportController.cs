using g3;
using Machine.ViewModels.Base;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MaterialRemove.ViewModels
{
    public abstract class PanelExportController : BaseViewModel, IPanelExportController
    {
        private IPanel _panel;
        public IPanel Panel 
        { 
            get => _panel; 
            set
            {
                if(Set(ref _panel, value, nameof(Panel)))
                { 
                    (_exportCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private ICommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new RelayCommand(() => ExportCommandImpl(), () => Panel != null));

        private void ExportCommandImpl()
        {
            if(GetFileName(out string fileName))
            {
                var list = new List<DMesh3>();

                foreach (var session in _panel.Sections)
                {
                    if(session.Volume != null) list.Add(GetGeometry(session.Volume));

                    foreach (var face in session.Faces) list.Add(GetGeometry(face));
                }

                StandardMeshWriter.WriteMeshes(fileName, list, WriteOptions.Defaults);
            }
        }

        private DMesh3 GetGeometry(ISectionElement element)
        {
            var ss = element as SectionElementViewModel;
            return ss.IsCorrupted ? ss.GetInternalGeometry() : ss.GenerateMesh();
        }

        protected abstract bool GetFileName(out string fileName);
    }
}
