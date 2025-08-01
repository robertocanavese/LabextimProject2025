using System;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.Services;
using AjaxControlToolkit;
using System.Collections.Generic;

using DLLabExtim;

namespace LabExtim
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutocompleteFilterService : System.Web.Services.WebService
    {
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
             MetaTable table = GetTable(contextKey);

            IQueryable queryable = BuildFilterQuery(table, prefixText, count);

            var values = new List<String>();
            foreach (var row in queryable)
            {
                values.Add(CreateAutoCompleteItem(table, row));
            }

            return values.ToArray();
        }

        private static IQueryable BuildFilterQuery(MetaTable table, string prefixText, int maxCount)
        {
            IQueryable query = table.GetQuery();

            // row
            var entityParam = Expression.Parameter(table.EntityType, "row");
            // row.DisplayName
            var property = Expression.Property(entityParam, table.DisplayColumn.Name);
            // "prefix"
            var constant = Expression.Constant(prefixText);
            // row.DisplayName.StartsWith("prefix")
            var startsWithCall = Expression.Call(property, typeof(string).GetMethod("Contains", new System.Type[] { typeof(string) }), constant);
            // row => row.DisplayName.StartsWith("prefix")
            var whereLambda = Expression.Lambda(startsWithCall, entityParam);
            // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
            var whereCall = Expression.Call(typeof(Queryable), "Where", new System.Type[] { table.EntityType }, query.Expression, whereLambda);
            // Customers.Where(row => row.DisplayName.StartsWith("prefix")).Take(20)
            var takeCall = Expression.Call(typeof(Queryable), "Take", new System.Type[] { table.EntityType }, whereCall, Expression.Constant(maxCount));

            if (table.Name == "Customers")
            {
                var constantCustomers = Expression.Constant("**");
                // row.DisplayName.StartsWith("prefix")
                var startsWithCallCustomers = Expression.And(Expression.Not(Expression.Call(property, typeof(string).GetMethod("Contains", new System.Type[] { typeof(string) }), constantCustomers)), startsWithCall);
                // row => row.DisplayName.StartsWith("prefix")
                var whereLambdaCustomers = Expression.Lambda(startsWithCallCustomers, entityParam);
                // Customers.Where(row => row.DisplayName.StartsWith("prefix"))
                var whereCallCustomers = Expression.Call(typeof(Queryable), "Where", new System.Type[] { table.EntityType }, query.Expression, whereLambdaCustomers);
                // Customers.Where(row => row.DisplayName.StartsWith("prefix")).Take(20)

                var orderByLambdaCustomers = Expression.Lambda<Func<Customer, IComparable>>(property, new ParameterExpression[] { entityParam });
                MethodCallExpression orderByCallCustomers = Expression.Call(typeof(Queryable), "OrderBy", new System.Type[] { table.EntityType, typeof(IComparable) },
                    whereCallCustomers, orderByLambdaCustomers);

                takeCall = Expression.Call(typeof(Queryable), "Take", new System.Type[] { table.EntityType }, orderByCallCustomers, Expression.Constant(maxCount));
            }




            return query.Provider.CreateQuery(takeCall);
        }

        public static string GetContextKey(MetaTable parentTable)
        {
            return String.Format("{0}#{1}", parentTable.DataContextType.FullName, parentTable.Name);
        }

        public static MetaTable GetTable(string contextKey)
        {
            string[] param = contextKey.Split('#');
            string[] assemblyQualifiedName = param[0].Split('.');
            Debug.Assert(param.Length == 2, String.Format("The context key '{0}' is invalid", contextKey));
            System.Type type = System.Type.GetType(param[0] + "," + assemblyQualifiedName[0]);
            return MetaModel.GetModel(type).GetTable(param[1], type);
        }

        private static string CreateAutoCompleteItem(MetaTable table, object row)
        {
            return AutoCompleteExtender.CreateAutoCompleteItem(table.GetDisplayString(row), table.GetPrimaryKeyString(row));
        }
    }
}
