using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace caMon.pages.TIS.CustomControls
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:caMon.pages.TIS"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:caMon.pages.TIS;assembly=caMon.pages.TIS"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:CommandIndicator/>
    ///
    /// </summary>
    public class CommandIndicator : Control
    {
        static CommandIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandIndicator), new FrameworkPropertyMetadata(typeof(CommandIndicator)));
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandIndicator()
        {
            this.Status = true;
            this.UpBackground = new SolidColorBrush(Colors.Blue);
            this.DownBackground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// 表示
        /// </summary>
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(
                "Status",
                typeof(bool),
                typeof(CommandIndicator),
                new UIPropertyMetadata(
                    false,
                    new PropertyChangedCallback(
                        (sender, e) =>
                            {
                                (sender as CommandIndicator).OnStatusPropertyChanged(sender, e);
                            }
                        )
                    )
            );
        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        private void OnStatusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                default:
                    break;
            }
        }

        /// <summary>
        /// 上側背景
        /// </summary>
        public static readonly DependencyProperty UpBackgroundProperty =
            DependencyProperty.Register(
                "UpBackground",
                typeof(Brush),
                typeof(CommandIndicator),
                new UIPropertyMetadata(Brushes.Transparent)
            );
        public Brush UpBackground
        {
            get { return (Brush)GetValue(UpBackgroundProperty); }
            set { SetValue(UpBackgroundProperty, value); }
        }

        /// <summary>
        /// 上側ボーダー
        /// </summary>
        public static readonly DependencyProperty UpBorderBrushProperty =
            DependencyProperty.Register(
                "UpBorderBrush",
                typeof(Brush),
                typeof(CommandIndicator),
                new UIPropertyMetadata(Brushes.Transparent)
            );
        public Brush UpBorderBrush
        {
            get { return (Brush)GetValue(UpBorderBrushProperty); }
            set { SetValue(UpBorderBrushProperty, value); }
        }

        /// <summary>
        /// 上側ボーダー線幅
        /// </summary>
        public static readonly DependencyProperty UpBorderThicknessProperty =
            DependencyProperty.Register(
                "UpBorderThickness",
                typeof(Thickness),
                typeof(CommandIndicator),
                new UIPropertyMetadata()
            );
        public Brush UpBorderThickness
        {
            get { return (Brush)GetValue(UpBorderThicknessProperty); }
            set { SetValue(UpBorderThicknessProperty, value); }
        }

        /// <summary>
        /// 下側背景
        /// </summary>
        public static readonly DependencyProperty DownBackgroundProperty =
            DependencyProperty.Register(
                "DownBackground",
                typeof(Brush),
                typeof(CommandIndicator),
                new UIPropertyMetadata(Brushes.Transparent)
            );
        public Brush DownBackground
        {
            get { return (Brush)GetValue(DownBackgroundProperty); }
            set { SetValue(DownBackgroundProperty, value); }
        }

        /// <summary>
        /// 下側ボーダー
        /// </summary>
        public static readonly DependencyProperty DownBorderBrushProperty =
            DependencyProperty.Register(
                "DownBorderBrush",
                typeof(Brush),
                typeof(CommandIndicator),
                new UIPropertyMetadata(Brushes.Transparent)
            );
        public Brush DownBorderBrush
        {
            get { return (Brush)GetValue(DownBorderBrushProperty); }
            set { SetValue(DownBorderBrushProperty, value); }
        }

        /// <summary>
        /// 下側ボーダー線幅
        /// </summary>
        public static readonly DependencyProperty DownBorderThicknessProperty =
            DependencyProperty.Register(
                "DownBorderThickness",
                typeof(Thickness),
                typeof(CommandIndicator),
                new UIPropertyMetadata()
            );
        public Brush DownBorderThickness
        {
            get { return (Brush)GetValue(DownBorderThicknessProperty); }
            set { SetValue(DownBorderThicknessProperty, value); }
        }
    }
}
