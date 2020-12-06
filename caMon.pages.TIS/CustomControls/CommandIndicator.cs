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
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static CommandIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandIndicator), new FrameworkPropertyMetadata(typeof(CommandIndicator)));
        }
        public CommandIndicator()
        {
        }


        /// <summary>
        /// 表示
        /// </summary>
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(
                "Status",
                typeof(bool),
                typeof(CommandIndicator),
                new UIPropertyMetadata(true)
            );
        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }


        /// <summary>
        /// 表示モード
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(
                "DisplayMode",
                typeof(int),
                typeof(CommandIndicator),
                new UIPropertyMetadata(
                    0,
                    OnDisplayModePropertyChanged
                    //coerceValueCallback
                    )
            );
        public int DisplayMode
        {
            get { return (int)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        private static void OnDisplayModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

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
        public Thickness UpBorderThickness
        {
            get { return (Thickness)GetValue(UpBorderThicknessProperty); }
            set { SetValue(UpBorderThicknessProperty, value); }
        }

        /// <summary>
        /// 上側表示文字
        /// </summary>
        public static readonly DependencyProperty UpDisplayProperty =
            DependencyProperty.Register(
                "UpDisplay",
                typeof(string),
                typeof(CommandIndicator),
                new UIPropertyMetadata("MNU")
            );
        public string UpDisplay
        {
            get { return (string)GetValue(UpDisplayProperty); }
            set { SetValue(UpDisplayProperty, value); }
        }

        /// <summary>
        /// 上側表示文字色
        /// </summary>
        public static readonly DependencyProperty UpDisplayBrushProperty =
            DependencyProperty.Register(
                "UpDisplayBrush",
                typeof(Brush),
                typeof(CommandIndicator),
                new UIPropertyMetadata(Brushes.White)
            );
        public Brush UpDisplayBrush
        {
            get { return (Brush)GetValue(UpDisplayBrushProperty); }
            set { SetValue(UpDisplayBrushProperty, value); }
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
        public Thickness DownBorderThickness
        {
            get { return (Thickness)GetValue(DownBorderThicknessProperty); }
            set { SetValue(DownBorderThicknessProperty, value); }
        }

        /// <summary>
        /// 下側表示文字
        /// </summary>
        public static readonly DependencyProperty DownDisplayProperty =
            DependencyProperty.Register(
                "DownDisplay",
                typeof(string),
                typeof(CommandIndicator),
                new UIPropertyMetadata("OFF")
            );
        public string DownDisplay
        {
            get { return (string)GetValue(DownDisplayProperty); }
            set { SetValue(DownDisplayProperty, value); }
        }

        /// <summary>
        /// 下側表示文字色
        /// </summary>
        public static readonly DependencyProperty DownDisplayBrushProperty =
            DependencyProperty.Register(
                "DownDisplayBrush",
                typeof(Brush),
                typeof(CommandIndicator),
                new UIPropertyMetadata(Brushes.Black)
            );
        public Brush DownDisplayBrush
        {
            get { return (Brush)GetValue(DownDisplayBrushProperty); }
            set { SetValue(DownDisplayBrushProperty, value); }
        }



        /// <summary>
        /// DependencyPropertyを指定の型と値で初期化
        /// </summary>
        /// <typeparam name="Type">DependencyPropertyの型</typeparam>
        /// <typeparam name="ParentType">DependencyPropertyを所有する型</typeparam>
        /// <param name="propertyName">DependencyPropertyの名前</param>
        /// <param name="propertyInitParam">DependencyPropertyの初期値</param>
        /// <returns>生成されたオブジェクト</returns>
        public static DependencyProperty RegisterDependencyProperty<Type, ParentType>(string propertyName, Type propertyInitParam)
        {
            return DependencyProperty.Register(
                propertyName,
                typeof(Type),
                typeof(ParentType),
                new UIPropertyMetadata(propertyInitParam)
            );
        }
        /// <summary>
        /// DependencyPropertyを指定の型と値で初期化
        /// </summary>
        /// <typeparam name="Type">DependencyPropertyの型</typeparam>
        /// <typeparam name="ParentType">DependencyPropertyを所有する型</typeparam>
        /// <param name="propertyName">DependencyPropertyの名前</param>
        /// <param name="propertyInitParam">DependencyPropertyの初期値</param>
        /// <param name="func"></param>
        /// <returns>生成されたオブジェクト</returns>
        /*
        public static DependencyProperty RegisterDependencyProperty<Type, ParentType>(string propertyName, Type propertyInitParam, Func<DependencyObject, DependencyPropertyChangedEventArgs, void> func)
        {
            return DependencyProperty.Register(
                propertyName,
                typeof(Type),
                typeof(ParentType),
                new UIPropertyMetadata(
                    propertyInitParam,
                    new PropertyChangedCallback(
                        (sender, e) => { func(sender, e); }
                        )
                    )
            );
        }
        */

        /// <summary>
        /// コード内でDependencyPropertyを扱うためのWrapper
        /// </summary>
        /// <typeparam name="Type">DependencyPropertyの型</typeparam>
        /// <param name="dependencyProperty">任意のDependencyProperty</param>
        /*
        public Type PropertyWrapper
        {
            get { return (Type)GetValue(dependencyProperty); }
            set { SetValue(dependencyProperty, value); }
        }
        */



        /// <summary>
        /// 一括で表示を変更
        /// 引数無しの場合は無表示
        /// </summary>
        /// <param name="up">上側表示文字</param>
        /// <param name="upBack">上側背景色</param>
        /// <param name="upbBrush">上側ボーダー色</param>
        /// <param name="upDispBrush">上側表示文字色</param>
        /// <param name="downBack">下側背景色</param>
        /// <param name="downbBrush">下側ボーダー色</param>
        /// <param name="downDispBrush">下側表示文字</param>
        private void ChangeDisplay(string up, Brush upBack, Brush upbBrush, Brush upDispBrush, Brush downBack, Brush downbBrush, Brush downDispBrush)
        {
            Status = true;
            ChangeDisplay(up, upBack, upDispBrush, downBack, downDispBrush);
            UpBorderBrush = upbBrush;
            DownBorderBrush = downbBrush;
            UpBorderThickness = new Thickness(5);
            DownBorderThickness = new Thickness(5);
        }
        private void ChangeDisplay(string up, Brush upBack, Brush upDispBrush, Brush downBack, Brush downDispBrush)
        {
            ChangeDisplay(up, upBack, upDispBrush);
            DownBackground = downBack;
            DownDisplayBrush = downDispBrush;
        }
        private void ChangeDisplay(string up, Brush upBack, Brush upDispBrush)
        {
            UpDisplay = up;
            UpBackground = upBack;
            UpDisplayBrush = upDispBrush;
            UpBorderThickness = new Thickness();
            DownBorderThickness = new Thickness();
        }
        private void ChangeDisplay()
        {
            Status = false;
        }
    }
}
