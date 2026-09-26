using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace CopilotTokenTracker
{
    /// <summary>
    ///  A small owner-drawn circular "spinner" used to indicate a pending operation.
    /// </summary>
    internal sealed class CircularSpinner : Control
    {
        private const int SweepAngle = 90;
        private const int StepAngle = 12;

        private readonly System.Windows.Forms.Timer _timer;
        private int _angle;
        private float _thickness = 3f;

        public CircularSpinner()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);

            _timer = new System.Windows.Forms.Timer { Interval = 30 };
            _timer.Tick += (_, _) =>
            {
                _angle = (_angle + StepAngle) % 360;
                Invalidate();
            };
        }

        [DefaultValue(3f)]
        public float Thickness
        {
            get => _thickness;
            set
            {
                _thickness = Math.Max(1f, value);
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Spinning
        {
            get => _timer.Enabled;
            set
            {
                if (value)
                {
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
                }

                Visible = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var inset = (_thickness / 2f) + 1f;
            var size = Math.Min(Width, Height) - (inset * 2f);

            if (size <= 0)
            {
                return;
            }

            var bounds = new RectangleF((Width - size) / 2f, (Height - size) / 2f, size, size);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var track = new Pen(Color.FromArgb(40, ForeColor), _thickness);
            using var arc = new Pen(ForeColor, _thickness) { StartCap = LineCap.Round, EndCap = LineCap.Round };

            e.Graphics.DrawEllipse(track, bounds);
            e.Graphics.DrawArc(arc, bounds, _angle, SweepAngle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
