using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using Windows.UI.Xaml;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CQControls.Helpers
{
    public class VisualTreeHelperTool
    {
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {

            Queue<DependencyObject> queue = new Queue<DependencyObject>();

            queue.Enqueue(obj);

            while (queue.Count > 0)

            {

                var item = queue.Dequeue();

                int count = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(item);

                for (int i = 0; i < count; i++)

                {

                    var re = Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(item, i);
                    
                    if (re is T)
                    {
                        return re as T;
                    }
                    else
                    {
                        queue.Enqueue(re);
                    }
                }
            }
            return null;
        }

        public static T FindNamedVisualChild<T>(DependencyObject obj, string name) where T : FrameworkElement

        {

            Queue<DependencyObject> queue = new Queue<DependencyObject>();

            queue.Enqueue(obj);

            while (queue.Count > 0)

            {

                var item = queue.Dequeue();

                int count = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(item);

                for (int i = 0; i < count; i++)

                {

                    var re = Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(item, i);

                    var child = re as FrameworkElement;

                    if (child != null && child.Name == name)

                    {

                        return child as T;

                    }

                    else

                    {

                        queue.Enqueue(child);

                    }

                }

            }





            return null;

        }

        public static T FindVisualParent<T>(DependencyObject obj) where T:FrameworkElement
        {
            var target = obj as FrameworkElement;
            if (target == null)
            {
                return null;
            }
            var parent = VisualTreeHelper.GetParent(target);
            if(parent is T)
            {
                return parent as T;
            }
            else
            {
                return FindVisualParent<T>(parent);
            }
        }

        public static T FindNamedVisualParent<T>(DependencyObject obj,string name) where T : FrameworkElement
        {
            var target = obj as FrameworkElement;
            if (target == null)
            {
                return null;
            }
            var parent = VisualTreeHelper.GetParent(target) as T;

            if (parent == null)
            {
                return null;
            }
            else
            {
                if (parent.Name == name)
                    return parent;
                else
                    return FindNamedVisualParent<T>(parent,name);
            }
        }


        public static VisualStateGroup FindVisualState(FrameworkElement element, string name)

        {

            if (element == null || string.IsNullOrWhiteSpace(name))

                return null;



            IList<VisualStateGroup> groups = VisualStateManager.GetVisualStateGroups(element);

            foreach (var group in groups)

            {

                if (group.Name == name)

                    return group;

            }



            return null;

        }

    }

}