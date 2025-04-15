using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BrotliUtility
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string inputFile;

        private string outPutPath;

        public string InputFile { get => inputFile; set => SetProperty(ref inputFile, value); }

        public string OutPutPath { get => outPutPath; set => SetProperty(ref outPutPath, value); }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private string resultContent;

        public string ResultContent { get => resultContent; set => SetProperty(ref resultContent, value); }
    }
}
