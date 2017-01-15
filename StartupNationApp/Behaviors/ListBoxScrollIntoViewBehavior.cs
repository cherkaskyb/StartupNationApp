using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace StartupNationApp.Behaviors
{
    public class ListBoxScrollIntoViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ((INotifyCollectionChanged)this.AssociatedObject.Items).CollectionChanged += ListBoxScrollIntoViewBehavior_CollectionChanged;
        }

        private void ListBoxScrollIntoViewBehavior_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var listbox = AssociatedObject as ListBox;
                var numOfValues = listbox.Items.Count;
                if (numOfValues > 0)
                {
                    listbox.ScrollIntoView(listbox.Items[numOfValues - 1]);
                }
            }
        }
    }
}
