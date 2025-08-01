using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using NotAClue.Web.DynamicData;
using NotAClue.Web;

namespace $rootnamespace$
{
    public partial class DateToFilter : System.Web.DynamicData.QueryableFilterUserControl
    {
        private String DATE_FORMAT = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

        public string DateFrom
        {
            get { return txbDateTo.Text; }
        }

        public override Control FilterControl
        {
            get { return txbDateTo; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // set correct date time format
            txbDateFrom_CalendarExtender.Format = DATE_FORMAT;

            if (!Column.ColumnType.Equals(typeof(DateTime)))
                throw new InvalidOperationException(String.Format("A date range filter was loaded for column '{0}' but the column has an incompatible type '{1}'.",
                    Column.Name, Column.ColumnType));

            Label1.Text = Column.DisplayName;

            txbDateTo.ToolTip = this.AppRelativeVirtualPath.GetFileNameTitle();
        }


        public override IQueryable GetQueryable(IQueryable source)
        {
            if (String.IsNullOrEmpty(DateFrom))
                return source;

            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, "item");
            Expression body = BuildQueryBody(parameterExpression);

            LambdaExpression lambda = Expression.Lambda(body, parameterExpression);
            MethodCallExpression whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery(whereCall);
        }

        private Expression BuildQueryBody(ParameterExpression parameterExpression)
        {
            Expression propertyExpression = LinqExpressionHelper.GetValue(LinqExpressionHelper.CreatePropertyExpression(parameterExpression, Column.Name));
            TypeConverter converter = TypeDescriptor.GetConverter(Column.ColumnType);
            BinaryExpression minimumComparison = BuildCompareExpression(propertyExpression, converter.ConvertFromString(DateFrom), Expression.LessThanOrEqual);
            return minimumComparison;
        }

        private BinaryExpression BuildCompareExpression(Expression propertyExpression, object value, Func<Expression, Expression, BinaryExpression> comparisonFunction)
        {
            ConstantExpression valueExpression = Expression.Constant(value);
            BinaryExpression comparison = comparisonFunction(propertyExpression, valueExpression);
            return comparison;
        }

        protected void btnRangeButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (button.ID == btnClear.ID)
                txbDateTo.Text = String.Empty;

            OnFilterChanged();
        }
    }
}
