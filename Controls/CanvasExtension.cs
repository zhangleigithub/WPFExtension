using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFExtension.Controls
{
    /// <summary>
    /// Canvas扩展
    /// </summary>
    public class CanvasExtension : Canvas
    {
        #region 字段

        /// <summary>
        /// 上一次鼠标按下去的位置
        /// </summary>
        private Point lastMouseDownPosition;

        /// <summary>
        /// 鼠标左键按下手型
        /// </summary>
        private Cursor handMouseDown;

        /// <summary>
        /// 鼠标左键移动按下手型
        /// </summary>
        private Cursor handMouseMove;

        #endregion

        #region 依赖属性

        /// <summary>
        /// X轴平移
        /// </summary>
        public double TranslateX
        {
            get { return (double)GetValue(TranslateXProperty); }
            set { SetValue(TranslateXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TranslateX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslateXProperty =
            DependencyProperty.Register("TranslateX", typeof(double), typeof(CanvasExtension), new PropertyMetadata(0.0));

        /// <summary>
        /// Y轴平移
        /// </summary>
        public double TranslateY
        {
            get { return (double)GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TranslateY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TranslateYProperty =
            DependencyProperty.Register("TranslateY", typeof(double), typeof(CanvasExtension), new PropertyMetadata(0.0));

        /// <summary>
        /// 缩放比例
        /// </summary>
        public double ScaleValue
        {
            get { return (double)GetValue(ScaleValueProperty); }
            set { SetValue(ScaleValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleValueProperty =
            DependencyProperty.Register("ScaleValue", typeof(double), typeof(CanvasExtension), new PropertyMetadata(1.0));

        /// <summary>
        /// 最小缩放比例
        /// </summary>
        public double MinScale
        {
            get { return (double)GetValue(MinScaleProperty); }
            set { SetValue(MinScaleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinScale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinScaleProperty =
            DependencyProperty.Register("MinScale", typeof(double), typeof(CanvasExtension), new PropertyMetadata(0.1));

        /// <summary>
        /// 最大缩放比例
        /// </summary>
        public double MaxScale
        {
            get { return (double)GetValue(MaxScaleProperty); }
            set { SetValue(MaxScaleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxScale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxScaleProperty =
            DependencyProperty.Register("MaxScale", typeof(double), typeof(CanvasExtension), new PropertyMetadata(10.0));

        /// <summary>
        /// 缩放间隔
        /// </summary>
        public double ScaleSpacing
        {
            get { return (double)GetValue(ScaleSpacingProperty); }
            set { SetValue(ScaleSpacingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleSpacing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleSpacingProperty =
            DependencyProperty.Register("ScaleSpacing", typeof(double), typeof(CanvasExtension), new PropertyMetadata(0.1));

        /// <summary>
        /// 获取附加属性
        /// </summary>
        /// <param name="dpo">依赖对象</param>
        /// <param name="value">值</param>
        public static object GetAttachedTag(DependencyObject dpo)
        {
            return dpo.GetValue(AttachedTagProperty);
        }

        /// <summary>
        /// 设置附加属性
        /// </summary>
        /// <param name="dpo">依赖对象</param>
        /// <param name="value">值</param>
        public static void SetAttachedTag(DependencyObject dpo, object value)
        {
            dpo.SetValue(AttachedTagProperty, value);
        }

        // Using a DependencyProperty as the backing store for AttachedTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachedTagProperty =
           DependencyProperty.RegisterAttached("AttachedTag", typeof(object), typeof(CanvasExtension),
           new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 鼠标左键按下时光标文件
        /// </summary>
        public string CursorFileMouseLeftButtonDown
        {
            get { return (string)GetValue(CursorFileMouseLeftButtonDownProperty); }
            set
            {
                SetValue(CursorFileMouseLeftButtonDownProperty, value);
                string directoryName = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
                this.SetCursorMouseLeftButtonDown(Path.Combine(directoryName, this.CursorFileMouseLeftButtonDown));
            }
        }

        // Using a DependencyProperty as the backing store for CursorFileMouseLeftButtonDown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CursorFileMouseLeftButtonDownProperty =
            DependencyProperty.Register("CursorFileMouseLeftButtonDown", typeof(string), typeof(CanvasExtension), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 鼠标移动时光标文件
        /// </summary>
        public string CursorFileMouseMove
        {
            get { return (string)GetValue(CursorFileMouseMoveProperty); }
            set
            {
                SetValue(CursorFileMouseMoveProperty, value);
                string directoryName = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
                this.SetCursorMouseMove(Path.Combine(directoryName, this.CursorFileMouseMove));
            }
        }

        // Using a DependencyProperty as the backing store for CursorFileMouseMove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CursorFileMouseMoveProperty =
            DependencyProperty.Register("CursorFileMouseMove", typeof(string), typeof(CanvasExtension), new PropertyMetadata(string.Empty));

        #endregion

        #region 事件

        /// <summary>
        /// 需要数据委托
        /// </summary>
        /// <param name="sender">源</param>
        /// <param name="e">参数</param>
        /// <param name="elements">UI元素集合</param>
        public delegate void NeedDataEventHandler(object sender, Rect e, List<UIElement> elements);

        /// <summary>
        /// 需要数据事件
        /// </summary>
        public event NeedDataEventHandler NeedData;

        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public CanvasExtension() : base()
        {
        }

        /// <summary>
        /// 设置鼠标左键按下时光标
        /// </summary>
        /// <param name="cursorFile">光标文件</param>
        public void SetCursorMouseLeftButtonDown(string cursorFile)
        {
            if (string.IsNullOrWhiteSpace(cursorFile))
            {
                throw new ArgumentNullException(nameof(cursorFile));
            }

            if (File.Exists(cursorFile))
            {
                this.handMouseDown = new Cursor(cursorFile);
            }
        }

        /// <summary>
        /// 设置鼠标移动时光标
        /// </summary>
        /// <param name="cursorFile">光标文件</param>
        public void SetCursorMouseMove(string cursorFile)
        {
            if (string.IsNullOrWhiteSpace(cursorFile))
            {
                throw new ArgumentNullException(nameof(cursorFile));
            }

            if (File.Exists(cursorFile))
            {
                this.handMouseMove = new Cursor(cursorFile);
            }
        }

        /// <summary>
        /// 重写属性更改
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == TranslateXProperty.Name || e.Property.Name == TranslateYProperty.Name)
            {
                this.UpdateChildTransform();
            }
            else if (e.Property.Name == ScaleValueProperty.Name)
            {
                this.UpdateChildTransform();
            }
            else if (e.Property.Name == CursorFileMouseLeftButtonDownProperty.Name)
            {
                string directoryName = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
                this.SetCursorMouseLeftButtonDown(Path.Combine(directoryName, this.CursorFileMouseLeftButtonDown));
            }
            else if (e.Property.Name == CursorFileMouseMoveProperty.Name)
            {
                string directoryName = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
                this.SetCursorMouseMove(Path.Combine(directoryName, this.CursorFileMouseMove));
            }
        }

        /// <summary>
        /// 重写OnMouseDown
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (e.ClickCount == 2)
                {
                    this.TranslateX = 0;
                    this.TranslateY = 0;
                    this.ScaleValue = 1;

                    //缩放+平移操作，将缩放后的鼠标参照物保持不变
                    foreach (UIElement item in this.Children.AsParallel())
                    {
                        Point orginLocation = this.GetOrginLocation(item);
                        TransformGroup transformGroup = this.GetTransformGroup(item);
                        ScaleTransform scaleTransform = this.GetScaleTransform(transformGroup);
                        scaleTransform.ScaleX = this.ScaleValue;
                        scaleTransform.ScaleY = this.ScaleValue;
                        TranslateTransform translateTransform = this.GetTranslateTransform(transformGroup);
                        translateTransform.X = this.TranslateX;
                        translateTransform.Y = this.TranslateY;
                    }
                }
            }
        }

        /// <summary>
        /// 重写OnMouseLeftButtonDown
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.lastMouseDownPosition = e.GetPosition(this);

            if (this.handMouseDown != null)
            {
                this.Cursor = this.handMouseDown;
            }
        }

        /// <summary>
        /// 重写OnMouseMove
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.handMouseMove != null)
                {
                    this.Cursor = this.handMouseMove;
                }

                Point p = e.GetPosition(this);
                double offsetX = p.X - lastMouseDownPosition.X;
                double offsetY = p.Y - lastMouseDownPosition.Y;
                this.MoveBehavior(offsetX, offsetY);
                this.lastMouseDownPosition = p;
            }
        }

        /// <summary>
        /// 重写OnMouseLeftButtonUp
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 重写OnMouseWheel
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            this.ZoomBehavior(e);
        }

        /// <summary>
        /// 移动行为
        /// </summary>
        /// <param name="offsetX">x轴偏移</param>
        /// <param name="offsetY">y轴偏移</param>
        private void MoveBehavior(double offsetX, double offsetY)
        {
            if (NeedData != null)
            {
                List<UIElement> uIElements = new List<UIElement>();

                //屏幕左侧移动
                if (offsetX > 0)
                {
                    NeedData(this, new Rect(this.TranslateX * -1 - offsetX, this.TranslateY, Math.Abs(offsetX), Math.Abs(offsetY)), uIElements);
                }
                else if (offsetX < 0) //屏幕右侧移动
                {
                    NeedData(this, new Rect(this.TranslateX * -1 + this.ActualWidth, this.TranslateY, Math.Abs(offsetX), Math.Abs(offsetY)), uIElements);
                }

                this.AddChild(uIElements);
            }

            //平移偏移量
            this.TranslateX += offsetX;
            this.TranslateY += offsetY;

            //更新
            this.UpdateChildTransform();
        }

        /// <summary>
        /// 缩放行为
        /// </summary>
        /// <param name="e">参数</param>
        private void ZoomBehavior(MouseWheelEventArgs e)
        {
            var canvasMousePoint = e.GetPosition(this);
            int delta = e.Delta / 120;

            if (this.ScaleValue + this.ScaleSpacing * delta < this.MinScale || this.ScaleValue + this.ScaleSpacing * delta > this.MaxScale)
            {
                return;
            }

            //缩小
            if (delta < 0)
            {
                if (NeedData != null)
                {
                    //可视范围内，可以容纳更多元素了，故需要加载更多数据
                    List<UIElement> uIElements = new List<UIElement>();
                    NeedData(this, new Rect(this.TranslateX * -1, this.TranslateY, this.ActualWidth, this.ActualHeight), uIElements);
                    this.AddChild(uIElements);
                }
            }

            //缩放 + 平移操作，将缩放后的鼠标参照物保持不变
            //模型鼠标位置
            Point modelMousePoint = new Point(canvasMousePoint.X - this.TranslateX, canvasMousePoint.Y - this.TranslateY);
            modelMousePoint.X /= this.ScaleValue;
            modelMousePoint.Y /= this.ScaleValue;

            //模型鼠标位置放大后
            var modelMousePointScaleTransformAfter = new Point(modelMousePoint.X * (1 + this.ScaleSpacing * delta), modelMousePoint.Y * (1 + this.ScaleSpacing * delta));

            //放大后，平移增量
            var translationIncrement = modelMousePoint - modelMousePointScaleTransformAfter;
            this.TranslateX += translationIncrement.X;
            this.TranslateY += translationIncrement.Y;

            //缩放值
            this.ScaleValue += this.ScaleSpacing * delta;

            //更新
            this.UpdateChildTransform();
        }

        /// <summary>
        /// 添加孩子节点
        /// </summary>
        /// <param name="elements">UI元素集合</param>
        private void AddChild(List<UIElement> elements)
        {
            foreach (UIElement item in elements.AsParallel())
            {
                this.Children.Add(item);
                this.SetOrginLocation(item);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void UpdateChildTransform()
        {
            //缩放+平移操作
            foreach (UIElement item in this.Children.AsParallel())
            {
                Point orginLocation = this.GetOrginLocation(item);
                TransformGroup transformGroup = this.GetTransformGroup(item);
                ScaleTransform scaleTransform = this.GetScaleTransform(transformGroup);
                scaleTransform.ScaleX = this.ScaleValue;
                scaleTransform.ScaleY = this.ScaleValue;
                TranslateTransform translateTransform = this.GetTranslateTransform(transformGroup);
                translateTransform.X = this.TranslateX + orginLocation.X * (this.ScaleValue - 1);
                translateTransform.Y = this.TranslateY + orginLocation.Y * (this.ScaleValue - 1);
            }
        }

        /// <summary>
        /// 获取原始位置
        /// </summary>
        /// <param name="element">UI元素</param>
        /// <returns>原始位置</returns>
        private Point GetOrginLocation(UIElement element)
        {
            this.SetOrginLocation(element);
            return (Point)CanvasExtension.GetAttachedTag(element);
        }

        /// <summary>
        /// 设置原始位置
        /// </summary>
        /// <param name="element">UI元素</param>
        private void SetOrginLocation(UIElement element)
        {
            if (CanvasExtension.GetAttachedTag(element) == null)
            {
                element.UpdateLayout();
                Point orginLocation = element.TransformToAncestor(this).Transform(new Point());
                CanvasExtension.SetAttachedTag(element, orginLocation);
            }
        }

        /// <summary>
        /// 获取转换集合
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns>转换集合</returns>
        private TransformGroup GetTransformGroup(UIElement element)
        {
            if (element.RenderTransform as TransformGroup == null)
            {
                element.RenderTransform = new TransformGroup();
            }

            return element.RenderTransform as TransformGroup;
        }

        /// <summary>
        /// 获取缩放对象
        /// </summary>
        /// <param name="transformGroup">转换集合</param>
        /// <returns>缩放对象</returns>
        private ScaleTransform GetScaleTransform(TransformGroup transformGroup)
        {
            ScaleTransform result = null;

            foreach (var item in transformGroup.Children)
            {
                if (item is ScaleTransform)
                {
                    result = item as ScaleTransform;
                    break;
                }
            }

            if (result == null)
            {
                result = new ScaleTransform();
                transformGroup.Children.Add(result);
            }

            return result;
        }

        /// <summary>
        /// 获取平移对象
        /// </summary>
        /// <param name="transformGroup">转换集合</param>
        /// <returns>平移对象</returns>
        private TranslateTransform GetTranslateTransform(TransformGroup transformGroup)
        {
            TranslateTransform result = null;

            foreach (var item in transformGroup.Children)
            {
                if (item is TranslateTransform)
                {
                    result = item as TranslateTransform;
                    break;
                }
            }

            if (result == null)
            {
                result = new TranslateTransform();
                transformGroup.Children.Add(result);
            }

            return result;
        }

        #endregion
    }
}
