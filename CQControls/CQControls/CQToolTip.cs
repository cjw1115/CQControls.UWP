using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CQControls
{
    
    public sealed class CQToolTip : Control
    {
        Popup root;
        ContentControl _contentControl;

        private FrameworkElement _targetControl;

        public FrameworkElement TargetControl
        {
            get { return _targetControl; }
            set
            {
                if (_targetControl != value)
                {
                    _targetControl = value;
                    //OnTargetControlPropertyChanged();
                }
                    
                
            }
        }

        TranslateTransform _translateTransform;
        private void OnTargetControlPropertyChanged()
        {

            if (TargetControl != null)
            {

                OnAlignmentPropertyChanged();
            }
            else
            {
                throw new NullReferenceException("Not set TargetControl value");
            }
            
        }

        public CQToolTip()
        {
            root = new Popup();
            _contentControl = new ContentControl();
            root.Child = _contentControl;
            _contentControl.SizeChanged += _contentControl_SizeChanged;
        }

        private void _contentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
            this.Height = e.NewSize.Height;

            OnTargetControlPropertyChanged();
        }

        public bool IsOpen
        {
            get { return (bool)this.GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }

        }
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(CQToolTip), new PropertyMetadata(null, IsOpenPropertyChangedCallback));
        public static void IsOpenPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as CQToolTip;
            sender.OnIsOpenPropertyChanged();
        }

        DispatcherTimer delayTimer;
        DispatcherTimer durationTimer;
        public void OnIsOpenPropertyChanged()
        {
            if (IsOpen)
            {
                if (DelayTime == 0)
                {
                    Open();
                }
                else
                {
                    delayTimer = new DispatcherTimer();
                    delayTimer.Interval = TimeSpan.FromMilliseconds(DelayTime);
                    delayTimer.Tick += DelayTick;
                    delayTimer.Start();
                }
            }
            else
            {
                if (delayTimer != null)
                {
                    delayTimer.Tick -= DelayTick;
                }
                if (durationTimer != null)
                {
                    durationTimer.Tick -= DurationTick;
                }

                delayTimer = null;
                durationTimer = null;

                root.IsOpen = false;

            }
        }
        private async void DelayTick(object o,object e)
        {
            await root.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Open();
            });

        }
        private async void DurationTick(object o,object e)
        {
            await root.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                root.IsOpen = false;
            });
        }
        private void Open()
        {
            root.IsOpen = true;
            if (IsForever != true)
            {
                durationTimer = new DispatcherTimer();
                durationTimer.Interval = TimeSpan.FromMilliseconds(Duration);
                durationTimer.Tick += DurationTick;
                durationTimer.Start();
            }
        }

        public object Content
        {
            get { return this.GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }

        }
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(CQToolTip), new PropertyMetadata(null, ContentPropertyChangedCallback));
        public static void ContentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender=d as CQToolTip;
            sender.OnContentPropertyChanged();
        }
        public void OnContentPropertyChanged()
        {
            _contentControl.Content = Content;
        }


        public new ControlTemplate Template
        {
            get { return this.GetValue(TemplateProperty) as ControlTemplate; }
            set { SetValue(TemplateProperty, value); }
        }
        public static new readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(CQToolTip), new PropertyMetadata(null, TemplatePropertyChangedCallback));
        public static void TemplatePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as CQToolTip;
            sender.OnPropertyChanged();
        }
        public void OnPropertyChanged()
        {
            _contentControl.Template = Template;
        }


        public static new readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register("HorizontalAlignment", typeof(HorizontalAlignment), typeof(CQToolTip), new PropertyMetadata(HorizontalAlignment.Left));
        public static new readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", typeof(VerticalAlignment), typeof(CQToolTip), new PropertyMetadata(VerticalAlignment.Top));
        public new HorizontalAlignment HorizontalAlignment
        {
            get { return (HorizontalAlignment)this.GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }
        public new VerticalAlignment VerticalAlignment
        {
            get { return (VerticalAlignment)this.GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }
        public static void AlignmentPropertyChangedCallBack(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            var sender = d as CQToolTip;
            sender.OnAlignmentPropertyChanged();
            
        }
        public void OnAlignmentPropertyChanged()
        {
            if (_translateTransform == null)
            {
                root.RenderTransform=_translateTransform = new TranslateTransform();
            }
            var targetPos = TargetControl.TransformToVisual((FrameworkElement)Window.Current.Content).TransformPoint(new Point(0, 0));
            var rootPos = targetPos;

            #region calc pos
            var offsetX = (this.TargetControl.ActualWidth - this.Width )/ 2 ;
            var offsetY = (this.TargetControl.ActualHeight - this.Height) / 2 ;
            if (this.HorizontalAlignment == HorizontalAlignment.Left && this.VerticalAlignment == VerticalAlignment.Top)
            {
                //rootPos.X += -this.Width;
                rootPos.Y += -this.Height;
            }
            else if (this.HorizontalAlignment == HorizontalAlignment.Left && (this.VerticalAlignment == VerticalAlignment.Center || this.VerticalAlignment == VerticalAlignment.Stretch))
            {
                rootPos.X += -this.Width;
                rootPos.Y += offsetY;
            }
            else if (this.HorizontalAlignment == HorizontalAlignment.Left && this.VerticalAlignment == VerticalAlignment.Bottom)
            {
                //rootPos.X += -this.Width;
                rootPos.Y += this.TargetControl.ActualHeight;
            }


            else if ((this.HorizontalAlignment == HorizontalAlignment.Center || this.HorizontalAlignment == HorizontalAlignment.Stretch) && this.VerticalAlignment == VerticalAlignment.Top)
            {
                rootPos.X += offsetX;
                rootPos.Y += -this.Height;
            }
            else if ((this.HorizontalAlignment == HorizontalAlignment.Center || this.HorizontalAlignment == HorizontalAlignment.Stretch) && (this.VerticalAlignment == VerticalAlignment.Center || this.VerticalAlignment == VerticalAlignment.Stretch))
            {
                rootPos.X += offsetX;
                rootPos.Y += offsetY;
            }
            else if ((this.HorizontalAlignment == HorizontalAlignment.Center || this.HorizontalAlignment == HorizontalAlignment.Stretch) && this.VerticalAlignment == VerticalAlignment.Bottom)
            {
                rootPos.X += offsetX;
                rootPos.Y += this.TargetControl.ActualHeight;
            }

            else if (this.HorizontalAlignment == HorizontalAlignment.Right && this.VerticalAlignment == VerticalAlignment.Top)
            {
                rootPos.X += this.TargetControl.ActualWidth-this.Width;
                rootPos.Y += -this.Height;
            }
            else if (this.HorizontalAlignment == HorizontalAlignment.Right && (this.VerticalAlignment == VerticalAlignment.Center || this.VerticalAlignment == VerticalAlignment.Stretch))
            {
                rootPos.X += this.TargetControl.ActualWidth;
                rootPos.Y += offsetY;
            }
            else if (this.HorizontalAlignment == HorizontalAlignment.Right && this.VerticalAlignment == VerticalAlignment.Bottom)
            {
                rootPos.X += this.TargetControl.ActualWidth - this.Width;
                rootPos.Y += this.TargetControl.ActualHeight;
            }
            #endregion

            _translateTransform.X = rootPos.X;
            _translateTransform.Y = rootPos.Y;

        }

        public static  readonly DependencyProperty DelayTimeProperty = DependencyProperty.Register("DelayTime", typeof(int), typeof(CQToolTip), new PropertyMetadata(0));
        public int DelayTime
        {
            get { return (int)this.GetValue(DelayTimeProperty); }
            set { SetValue(DelayTimeProperty, value); }
        }
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(int), typeof(CQToolTip), new PropertyMetadata(3000));
        public int Duration
        {
            get { return (int)this.GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty IsForeverProperty = DependencyProperty.Register("IsForever", typeof(bool), typeof(CQToolTip), new PropertyMetadata(false));
        public bool IsForever
        {
            get { return (bool)this.GetValue(IsForeverProperty); }
            set { SetValue(IsForeverProperty, value); }
        }

    }

    public static class CQToolTipService
    {
        #region Attached Propertie
        public static CQToolTip GetCQToolTip(DependencyObject d)
        {
            return d.GetValue(CQToolTipProperty) as CQToolTip;
        }
        public static void SetCQToolTip(DependencyObject d, CQToolTip value)
        {
            d.SetValue(CQToolTipProperty, value);
        }
        public static readonly DependencyProperty CQToolTipProperty = DependencyProperty.RegisterAttached("CQToolTip", typeof(CQToolTip), typeof(UIElement), new PropertyMetadata(null, CQToolTipPropertyChangedCallback));
        public static void CQToolTipPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rootElement = d as UIElement;
            if (rootElement != null)
            {
                rootElement.PointerEntered -= RootElement_PointerEntered;
                rootElement.PointerExited -= RootElement_PointerExited;

                rootElement.PointerEntered += RootElement_PointerEntered;
                rootElement.PointerExited += RootElement_PointerExited;
            }
        }

        private static void RootElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            
            var d = sender as DependencyObject;
            var tooltip = d.GetValue(CQToolTipProperty) as CQToolTip;
            if (tooltip != null)
            {
                tooltip.IsOpen = false;
            }
        }

        private static void RootElement_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var d = sender as FrameworkElement;
            var tooltip = d.GetValue(CQToolTipProperty) as CQToolTip;
            if (tooltip != null)
            {
                tooltip.TargetControl = d;
                tooltip.IsOpen = true;
            }
        }

        #endregion

        
    }

}
