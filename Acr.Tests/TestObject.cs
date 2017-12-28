using System;


namespace Acr.Tests
{
    public class TestObject : NotifyPropertyChanged
    {

        string value;
        public string Value
        {
            get => this.value;
            set => this.SetProperty(ref this.value, value);
        }
    }
}
