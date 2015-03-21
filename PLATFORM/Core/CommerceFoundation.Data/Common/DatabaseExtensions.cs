using System;
using System.Data.Entity;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;

namespace VirtoCommerce.Foundation.Data.Common
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Creates the index of the unique.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <param name="expression">The expression.</param>
        /// <exception cref="System.ArgumentNullException">database</exception>
        public static void CreateUniqueIndex<TModel>(this Database database, Expression<Func<TModel, object>> expression)
        {
            if (database == null)
                throw new ArgumentNullException("database");

            // Assumes singular table name matching the name of the Model type

            var tableName = typeof(TModel).GetCustomAttributes(true).OfType<EntitySetAttribute>().Single().EntitySet;
            //var tableName = typeof(TModel).Name;
            var columnName = GetLambdaExpressionName(expression.Body);
            var indexName = string.Format("UIX_{0}_{1}", tableName, columnName);

            var createIndexSql = string.Format("CREATE UNIQUE INDEX {0} ON {1} ({2})", indexName, tableName, columnName);

            database.ExecuteSqlCommand(createIndexSql);
        }

        /// <summary>
        /// Creates the index.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="database">The database.</param>
        /// <param name="expression">The expression.</param>
        /// <exception cref="System.ArgumentNullException">database</exception>
        public static void CreateIndex<TModel>(this Database database, Expression<Func<TModel, object>> expression)
        {
            if (database == null)
                throw new ArgumentNullException("database");

            // Assumes singular table name matching the name of the Model type

            var tableName = typeof(TModel).GetCustomAttributes(true).OfType<EntitySetAttribute>().Single().EntitySet;
            //var tableName = typeof(TModel).Name;
            var columnName = GetLambdaExpressionName(expression.Body);
            var indexName = string.Format("IX_{0}_{1}", tableName, columnName);

            var createIndexSql = string.Format("CREATE INDEX {0} ON {1} ({2})", indexName, tableName, columnName);

            database.ExecuteSqlCommand(createIndexSql);
        }

        /// <summary>
        /// Gets the name of the lambda expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Cannot get name from expression;expression</exception>
        public static string GetLambdaExpressionName(Expression expression)
        {
            var memberExp = expression as MemberExpression;

            if (memberExp == null)
            {
                // Check if it is an UnaryExpression and unwrap it
                var unaryExp = expression as UnaryExpression;
                if (unaryExp != null)
                    memberExp = unaryExp.Operand as MemberExpression;
            }

            if (memberExp == null)
                throw new ArgumentException("Cannot get name from expression", "expression");

            return memberExp.Member.Name;
        }
    }
}
