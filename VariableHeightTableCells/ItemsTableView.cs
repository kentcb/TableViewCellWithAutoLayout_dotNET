using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace VariableHeightTableCells
{
    public class ItemsTableView : UITableViewController
    {
        private readonly Model model;
        private ItemCell offscreenCell;
        private bool isInsertingRow;
        private NSObject contentSizeCategoryChangedObserver;

        public ItemsTableView()
        {
            this.model = new Model();
            this.model.PopulateItems();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.TableView.RegisterClassForCellReuse(typeof(ItemCell), ItemCell.Key);

            this.Title = "Auto Layout Table View";
            this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Trash, this.Clear);
            this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, this.AddRow);

            this.TableView.AllowsSelection = false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.contentSizeCategoryChangedObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.ContentSizeCategoryChangedNotification, this.ContentSizeCategoryChanged);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (this.contentSizeCategoryChangedObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(this.contentSizeCategoryChangedObserver);
            }
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return this.model.Items.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (ItemCell)tableView.DequeueReusableCell(ItemCell.Key);

            cell.UpdateFonts();

            var item = this.model.Items[indexPath.Row];
            cell.Title = item.Title;
            cell.Body = item.Body;

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            return cell;
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (this.offscreenCell == null)
            {
                this.offscreenCell = new ItemCell();
            }

            var cell = this.offscreenCell;

            cell.UpdateFonts();
            var item = this.model.Items[indexPath.Row];
            cell.Title = item.Title;
            cell.Body = item.Body;

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            cell.Bounds = new RectangleF(0, 0, this.TableView.Bounds.Width, this.TableView.Bounds.Height);

            cell.SetNeedsLayout();
            cell.LayoutIfNeeded();

            var height = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;
            height += 1;

            return height;
        }

        public override float EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        {
            // NOTE for iOS 7.0.x ONLY, this bug has been fixed by Apple as of iOS 7.1:
            // A constraint exception will be thrown if the estimated row height for an inserted row is greater
            // than the actual height for that row. In order to work around this, we need to return the actual
            // height for the the row when inserting into the table view - uncomment the below 3 lines of code.
            // See: https://github.com/caoimghgin/TableViewCellWithAutoLayout/issues/6
//            if (this.isInsertingRow)
//            {
//                return this.GetHeightForRow(tableView, indexPath);
//            }

            return UITableView.AutomaticDimension;
        }

        private void ContentSizeCategoryChanged(NSNotification notification)
        {
            this.TableView.ReloadData();
        }

        private void Clear(object sender, EventArgs e)
        {
            this.model.Items.Clear();
            this.TableView.ReloadData();
        }

        private void AddRow(object sender, EventArgs e)
        {
            this.isInsertingRow = true;

            try
            {
                this.model.AddSingleItem();
                var indexPath = NSIndexPath.Create(0, this.model.Items.Count - 1);
                this.TableView.InsertRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
            }
            finally
            {
                this.isInsertingRow = false;
            }
        }
    }
}