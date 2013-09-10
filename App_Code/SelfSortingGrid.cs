using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIU.Web.UI.WebControls
{
    public class SelfSortingGrid : GridView
    {
        private string _lastAscendingSort;

        protected override void OnSorting(GridViewSortEventArgs e)
        {
            this.Sorting += new GridViewSortEventHandler(this.SelfSortingGrid_Sorting);

            base.OnSorting(e);
        }

        private void SelfSortingGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == this._lastAscendingSort)
            {
                this._lastAscendingSort = null;

                e.SortDirection = SortDirection.Descending;
            }
            else
            {
                this._lastAscendingSort = e.SortExpression;
            }

            IEnumerable _data = null;

            // This will allow LINQ to SQL to pass the sorting on to the SQL Server itself.
            if (this.DataSource is IQueryable)
            {
                _data = (IQueryable) this.DataSource;
            }
            else
            {
                DataSourceView dataView = this.GetData();

                if (dataView == null)
                {
                    // No data to sort.
                    return;
                }

                dataView.Select(this.SelectArguments, delegate(IEnumerable data)
                                                          {
                                                              _data = data;
                                                          });

                if (_data == null)
                {
                    // No data to sort.
                    return;
                }
            }

            Type dataSourceType = _data.GetType();

            Type dataItemType = typeof (object);

            if (dataSourceType.HasElementType)
            {
                dataItemType = dataSourceType.GetElementType();
            }
            else if (dataSourceType.IsGenericType)
            {
                dataItemType = dataSourceType.GetGenericArguments()[0];
            }
            else if (_data is IEnumerable)
            {
                IEnumerator dataEnumerator = _data.GetEnumerator();

                if (dataEnumerator.MoveNext() && dataEnumerator.Current != null)
                {
                    dataItemType = dataEnumerator.Current.GetType();
                }
            }

            //var fieldType = dataItemType.GetProperty(e.SortExpression);

            object sorterObject = null;

            Type sorterType = null;

            // We'll handle things like LINQ to SQL differently by passing the love
            // on to the provider.
            PropertyInfo property = dataItemType.GetProperty(e.SortExpression);

            if (property != null)
            {
                sorterType = typeof (GenericSorter<,>).MakeGenericType(dataItemType, property.PropertyType);

                sorterObject = Activator.CreateInstance(sorterType);
            }
            else
            {
                sorterType = typeof (LateBoundSorter);

                sorterObject = Activator.CreateInstance(sorterType);
            }

            this.DataSource = sorterType.GetMethod("Sort",
                                                   new Type[] {dataSourceType, typeof (string), typeof (SortDirection)})
                .Invoke(sorterObject, new object[] {_data, e.SortExpression, e.SortDirection});

            this.DataBind();
        }

        protected override object SaveControlState()
        {
            return new object[] {this._lastAscendingSort, base.SaveControlState()};
        }

        protected override void LoadControlState(object savedState)
        {
            object[] stateItems = savedState as object[];

            if (stateItems != null && stateItems.Length == 2)
            {
                this._lastAscendingSort = stateItems[0] as string;

                base.LoadControlState(stateItems[1]);
            }
            else
            {
                base.LoadControlState(savedState);
            }
        }
    }

    public class GenericSorter<T, PT>
    {
        public IEnumerable<T> Sort(IEnumerable source, string sortExpression, SortDirection sortDirection)
        {
            var param = Expression.Parameter(typeof (T), "item");

            var sortLambda =
                Expression.Lambda<Func<T, PT>>(
                    Expression.Convert(Expression.Property(param, sortExpression), typeof (PT)), param);

            if (sortDirection == SortDirection.Ascending)
            {
                return source.OfType<T>().AsQueryable<T>().OrderBy<T, PT>(sortLambda);
            }
            else
            {
                return source.OfType<T>().AsQueryable<T>().OrderByDescending<T, PT>(sortLambda);
            }
        }

        public IEnumerable<T> Sort(IEnumerable<T> source, string sortExpression, SortDirection sortDirection)
        {
            var param = Expression.Parameter(typeof (T), "item");

            var sortLambda =
                Expression.Lambda<Func<T, PT>>(
                    Expression.Convert(Expression.Property(param, sortExpression), typeof (PT)), param);

            if (sortDirection == SortDirection.Ascending)
            {
                return source.AsQueryable<T>().OrderBy<T, PT>(sortLambda);
            }
            else
            {
                return source.AsQueryable<T>().OrderByDescending<T, PT>(sortLambda);
            }
        }
    }

    public class LateBoundSorter
    {
        public IEnumerable Sort(IEnumerable source, string sortExpression, SortDirection sortDirection)
        {
            if (sortDirection == SortDirection.Ascending)
            {
                return source.OfType<object>().OrderBy(item =>
                                                       this.GetPropertyValue(item, sortExpression));
            }
            else
            {
                return source.OfType<object>().OrderByDescending(item =>
                                                                 this.GetPropertyValue(item, sortExpression));
            }
        }

        private object GetPropertyValue(object item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName).GetValue(item, null);
        }
    }
}