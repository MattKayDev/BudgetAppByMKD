using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiBudgetApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy;
        string title;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                if (this.isBusy == value)
                {
                    return;
                }
                isBusy = value;
                OnPropertyChanged();
                // also raising the IsNotBusy property changed
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy => !IsBusy;

        public string Title
        {
            get => this.title;
            set
            {
                if(this.title == value)
                {
                    return;
                }
                this.title = value;
                OnPropertyChanged();
            }
        }
    }
}
