using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClickYeeter9000.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ClickYeeter9000.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ClickYeeter9000.Controls;assembly=ClickYeeter9000.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:GlowBorder/>
    ///
    /// </summary>
    public class GlowBorder : ContentControl
    {
        static GlowBorder() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowBorder), new FrameworkPropertyMetadata(typeof(GlowBorder)));
        }

        public CornerRadius CornerRadius {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(GlowBorder), new PropertyMetadata(new CornerRadius()));

        public double BorderOpacity {
            get { return (double)GetValue(BorderOpacityProperty); }
            set { SetValue(BorderOpacityProperty, value); }
        }

        public static readonly DependencyProperty BorderOpacityProperty =
            DependencyProperty.Register("BorderOpacity", typeof(double), typeof(GlowBorder), new PropertyMetadata((double)1));

        public Brush BorderBackground {
            get { return (Brush)GetValue(BorderBackgroundProperty); }
            set { SetValue(BorderBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BorderBackgroundProperty =
            DependencyProperty.Register("BorderBackground", typeof(Brush), typeof(GlowBorder), new PropertyMetadata(Brushes.Transparent));

        public GlowBorder() {
        }
    }
}
