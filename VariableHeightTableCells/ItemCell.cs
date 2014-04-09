using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace VariableHeightTableCells
{
    public class ItemCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ItemCell");

        private UILabel titleLabel;
        private UILabel bodyLabel;
        private bool didSetupConstraints;

        public ItemCell()
        {
            this.CreateView();
        }

        public ItemCell(IntPtr handle)
            : base(handle)
        {
            this.CreateView();
        }

        public string Title
        {
            get { return this.titleLabel.Text; }
            set { this.titleLabel.Text = value; }
        }

        public string Body
        {
            get { return this.bodyLabel.Text; }
            set { this.bodyLabel.Text = value; }
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }

            // this avoids an unsatisfiable constraint problem where the constraints result in a cell size larger than the current size (probably 320 x 44)
            // see here for more information: https://github.com/Alex311/TableCellWithAutoLayout/commit/bde387b27e33605eeac3465475d2f2ff9775f163#commitcomment-4633188
            //this.ContentView.Bounds = new RectangleF(0, 0, 100000, 100000);

            this.titleLabel.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            this.bodyLabel.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);

            this.ContentView.ConstrainLayout(() =>
                this.titleLabel.Top() == this.ContentView.Top() + Layout.StandardSiblingViewSpacing &&
                this.titleLabel.Left() == this.ContentView.Left() + Layout.StandardSiblingViewSpacing &&
                this.titleLabel.Right() == this.ContentView.Right() - Layout.StandardSiblingViewSpacing &&
                this.bodyLabel.Top() >= this.titleLabel.Bottom() + Layout.StandardSiblingViewSpacing &&
                this.bodyLabel.Left() == this.ContentView.Left() + Layout.StandardSiblingViewSpacing &&
                this.bodyLabel.Right() == this.ContentView.Right() - Layout.StandardSiblingViewSpacing &&
                this.bodyLabel.Bottom() == this.ContentView.Bottom() - Layout.StandardSiblingViewSpacing);

            this.didSetupConstraints = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.ContentView.SetNeedsLayout();
            this.ContentView.LayoutIfNeeded();

            this.bodyLabel.PreferredMaxLayoutWidth = this.bodyLabel.Frame.Width;
        }

        public void UpdateFonts()
        {
            this.titleLabel.Font = UIFont.PreferredHeadline;
            this.bodyLabel.Font = UIFont.PreferredCaption2;
        }

        private void CreateView()
        {
            this.titleLabel = new UILabel {
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 1,
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.Black,
                BackgroundColor = UIColor.FromRGBA(0, 0, 255, 30)
            };

            this.bodyLabel = new UILabel {
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.DarkGray,
                BackgroundColor = UIColor.FromRGBA(255, 0, 0, 30)
            };

            this.ContentView.BackgroundColor = UIColor.FromRGBA(0, 255, 0, 30);
            this.ContentView.AddSubviews(this.titleLabel, this.bodyLabel);

            this.UpdateFonts();
        }
    }
}