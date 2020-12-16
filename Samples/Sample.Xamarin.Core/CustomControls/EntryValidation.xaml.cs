using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.Xamarin.Core.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryValidation : ContentView
    {
        public EntryValidation()
        {
            InitializeComponent();
            EntryInput.TextChanged += EntryInput_TextChanged;
        }

        ~EntryValidation()
        {
            EntryInput.TextChanged -= EntryInput_TextChanged;
        }

        private void EntryInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = EntryInput.Text;
            Textchanged?.Execute(null);
        }

        public static readonly BindableProperty InvalidProperty = BindableProperty.Create(
            "Invalid",
            typeof(bool),
            typeof(EntryValidation),
            false,
            BindingMode.TwoWay,
            propertyChanged: InvalidPropertyChanged
            );

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
              "Text",
              typeof(string),
              typeof(EntryValidation),
              null,
              BindingMode.TwoWay,
              propertyChanged: TextPropertyChanged);

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
              "Placeholder",
              typeof(string),
              typeof(EntryValidation),
              null,
              BindingMode.TwoWay,
              propertyChanged: PlaceholderPropertyChanged);

        public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create(
                "ErrorMessage",
                typeof(string),
                typeof(EntryValidation),
                null,
                BindingMode.TwoWay,
                propertyChanged: ErrorMessagePropertyChanged);

        public static readonly BindableProperty TextchangedProperty = BindableProperty.Create(
            "Textchanged",
            typeof(ICommand),
            typeof(EntryValidation),
            null,
            BindingMode.OneWay);

        private static void InvalidPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EntryValidation)bindable;
            control.Invalid = (bool)newValue;
            control.ErrorLabel.IsVisible = control.Invalid;
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EntryValidation)bindable;
            control.Text = (string)newValue;
            control.EntryInput.Text = control.Text;
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EntryValidation)bindable;
            control.Placeholder = (string)newValue;
            control.EntryInput.Placeholder = control.Placeholder;
        }

        private static void ErrorMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EntryValidation)bindable;
            control.ErrorMessage = (string)newValue;
            control.ErrorLabel.Text = control.ErrorMessage;
        }

        public bool Invalid
        {
            get => (bool)GetValue(InvalidProperty);
            set
            {
                SetValue(InvalidProperty, value);
            }
        }
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public ICommand Textchanged
        {
            get => (ICommand)GetValue(TextchangedProperty);
            set => SetValue(TextchangedProperty, value);
        }
    }
}