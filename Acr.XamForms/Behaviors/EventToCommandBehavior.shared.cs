using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;


namespace Acr.XamForms.Behaviors
{
    public class EventToCommandBehavior : AbstractBindableBehavior<View>
    {
        Delegate eventHandler;


        public static readonly BindableProperty EventNameProperty = BindableProperty.Create(
            nameof(EventName),
            typeof(string),
            typeof(EventToCommandBehavior),
            null,
            propertyChanged: OnEventNameChanged
        );
        public string EventName
        {
            get => (string)this.GetValue(EventNameProperty);
            set => this.SetValue(EventNameProperty, value);
        }


        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(EventToCommandBehavior)
        );
        public ICommand Command
        {
            get => (ICommand) this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }


        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(EventToCommandBehavior)
        );
        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }


        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create(
            nameof(Converter),
            typeof(IValueConverter),
            typeof(EventToCommandBehavior)
        );
        public IValueConverter Converter
        {
            get => (IValueConverter) this.GetValue(InputConverterProperty);
            set => this.SetValue(InputConverterProperty, value);
        }


        protected override void OnAttachedTo(View view)
        {
            base.OnAttachedTo(view);
            this.RegisterEvent(this.EventName);
        }


        protected override void OnDetachingFrom(View view)
        {
            this.DeregisterEvent(this.EventName);
            base.OnDetachingFrom(view);
        }


        void RegisterEvent(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return;

            var eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
                throw new ArgumentException($"EventToCommandBehavior: Can't register the '{this.EventName}' event.");

            var methodInfo = typeof(EventToCommandBehavior)
                .GetTypeInfo()
                .GetDeclaredMethod("OnEvent");

            this.eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(this.AssociatedObject, this.eventHandler);
        }


        void DeregisterEvent(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return;

            if (this.eventHandler == null)
                return;

            var eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
                throw new ArgumentException($"EventToCommandBehavior: Can't de-register the '{this.EventName}' event.");

            eventInfo.RemoveEventHandler(this.AssociatedObject, this.eventHandler);
        }


        void OnEvent(object sender, object eventArgs)
        {
            if (this.Command == null)
                return;

            object resolvedParameter = null;
            if (this.CommandParameter != null)
                resolvedParameter = this.CommandParameter;

            else if (this.Converter != null)
                resolvedParameter = this.Converter.Convert(eventArgs, typeof(object), null, null);
            //else
                //resolvedParameter = eventArgs;

            if (this.Command.CanExecute(resolvedParameter))
                this.Command.Execute(resolvedParameter);
        }


        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
                return;

            behavior.DeregisterEvent((string)oldValue);
            behavior.RegisterEvent((string)newValue);
        }
    }
}
