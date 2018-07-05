using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace Acr.ComponentModel
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("Expression is not a MemberExpression");

            this.OnPropertyChanged(member.Member.Name);
        }


        protected virtual bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
                return false;

            property = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
