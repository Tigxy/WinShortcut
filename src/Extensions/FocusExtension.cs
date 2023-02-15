using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace win_short_cut.Extensions {
    // see https://stackoverflow.com/a/7972361
    public static class FocusExtension {
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
                name: "IsFocused",
                propertyType: typeof(bool?),
                ownerType: typeof(FocusExtension),
                defaultMetadata: new FrameworkPropertyMetadata(IsFocusedChanged) { BindsTwoWayByDefault = true }
                );

        public static bool? GetIsFocused(DependencyObject element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool?)element.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject element, bool? value) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(IsFocusedProperty, value);
        }

        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null) {
                fe.GotFocus += FrameworkElement_GotFocus;
                fe.LostFocus += FrameworkElement_LostFocus;
            }

            if (!fe.IsVisible) {
                fe.IsVisibleChanged += new DependencyPropertyChangedEventHandler(fe_IsVisibleChanged);
            }

            if (e.NewValue != null && (bool)e.NewValue) {
                FocusElement(fe);
            }
        }

        private static void fe_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var fe = (FrameworkElement)sender;
            if (fe.IsVisible && (bool)fe.GetValue(IsFocusedProperty)) {
                fe.IsVisibleChanged -= fe_IsVisibleChanged;
                FocusElement(fe);
            }
        }

        private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e) {
            if (e.Source == e.OriginalSource)
                ((FrameworkElement)sender).SetValue(IsFocusedProperty, true);
        }

        private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e) {
            if (e.Source == e.OriginalSource)
                ((FrameworkElement)sender).SetValue(IsFocusedProperty, false);
        }

        private static void FocusElement(FrameworkElement fe) {
            fe.Focus();
            Keyboard.Focus(fe);

            if (fe is TextBox textbox)
                textbox.CaretIndex = textbox.Text.Length;
        }
    }
}
