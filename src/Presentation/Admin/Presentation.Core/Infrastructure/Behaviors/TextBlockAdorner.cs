using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors
{
    public class TextBlockAdorner : Adorner
    {
        private readonly TextBlock _textBlock;

        public TextBlockAdorner(UIElement adornedElement, string label, Style labelStyle)
            : base(adornedElement)
        {
            _textBlock = new TextBlock { Style = labelStyle, Text = label };
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _textBlock.Measure(constraint);
            return _textBlock.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _textBlock.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _textBlock;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}