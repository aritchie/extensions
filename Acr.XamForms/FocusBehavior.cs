using System;
using Xamarin.Forms;


namespace Acr.XamForms
{
    public class FocusBehavior : AbstractBindableBehavior<View>
    {
        protected override void OnAttachedTo(View view)
        {
            view.FocusChangeRequested += this.OnFocusChangeRequested;
            base.OnAttachedTo(view);
        }


        protected override void OnDetachingFrom(View view)
        {
            view.FocusChangeRequested -= this.OnFocusChangeRequested;
            base.OnDetachingFrom(view);
        }


        public static readonly BindableProperty IsFocusedProperty = BindableProperty.Create(
            nameof(IsFocused),
            typeof(string),
            typeof(FocusBehavior),
            null,
            propertyChanged: OnPropertyChanged
        );
        public bool IsFocused
        {
            get => (bool) this.GetValue(IsFocusedProperty);
            set => this.SetValue(IsFocusedProperty, value);
        }


        static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (FocusBehavior)bindable;
            if (behavior.AssociatedObject == null)
                return;

            var changed = behavior.AssociatedObject.IsFocused != (bool) newValue;
            if (!changed)
                return;

            if ((bool) newValue)
            {
                behavior.AssociatedObject.Focus();
            }
            else
            {
                behavior.AssociatedObject.Unfocus();
            }
        }


        void OnFocusChangeRequested(object sender, VisualElement.FocusRequestArgs e)
            => this.IsFocused = e.Focus;
    }
}
