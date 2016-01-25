using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Backendless.Core
{
	public class BackendlessQuery<T> : IBackendlessQuery<T> where T: BackendlessEntity
	{


		public BackendlessQuery(){
			QueryType = BackendlessQueryType.Advanced;
			DateTimeQueryFormat = "o";
		}

		internal BackendlessQuery(string query,BackendlessQueryType queryType = BackendlessQueryType.Basic){
			Query = query;
			QueryType = queryType;
		}


		public static BackendlessQuery<T> FirstQuery{
			get{
				return new BackendlessQuery<T> ("/first");
			}
		}

		public static BackendlessQuery<T> LastQuery{
			get{
				return new BackendlessQuery<T> ("/last");
			}
		}

		public static BackendlessQuery<T> AllCollectionQuery {
			get {
				return new BackendlessQuery<T> ("/", BackendlessQueryType.Advanced);
			}
		}

		public static BackendlessQuery<T> FromObjectIdQuery(string objectId){
			return new BackendlessQuery<T> (string.Concat ("/",objectId));
		}

		#region IBackendlessQuery implementation

		public IList<string> Props { get; private set;}

		public IList<string> SortBy { get; private set;}

		public string WhereToString {
			get {
				return ParseFilterProvider.Parse (Where, DateTimeQueryFormat);
			}
		}

		#endregion

		#region Private

		public BackendlessQueryType QueryType { get; private set;}

		public string Query { get; private set;}

		#endregion

		#region Public 

		public int? Limit { get; set;}

		public uint Offset { get; set;}

		public string DateTimeQueryFormat { get; set;}

		public Expression<Func<T,bool>>  Where { get; set;}

		#endregion

	



		public BackendlessQuery<T> SetSortBy<P>(Expression<Func<T,P>> prop){
			var member = prop.Body as MemberExpression;
			SortBy = SortBy ?? new List<string> ();
			SortBy.Add (MemberNameToSearchName (member));
			return this;
		}



		public BackendlessQuery<T> SetProp<P>(Expression<Func<T,P>> prop){
			var member = prop.Body as MemberExpression;
			Props = Props ?? new List<string> ();
			Props.Add (MemberNameToSearchName (member));
			return this;
		}
			
		static string MemberNameToSearchName(MemberExpression member){
			var customAttributes = member.Member.CustomAttributes.ToList ();
			var def = customAttributes.FirstOrDefault (x => x.AttributeType == typeof(JsonPropertyAttribute));
			return def != null ? def.ConstructorArguments [0].Value.ToString () : member.Member.Name;
		}


		class ParseFilterProvider {

		string dateTimeQueryFormat;
		readonly StringBuilder sb = new StringBuilder();
			public static string Parse(Expression expr,string dateTimeQueryFormat){
				var p = new ParseFilterProvider (dateTimeQueryFormat);
				return p.ParseFilter (expr);
			
			}

			public ParseFilterProvider(string dateTimeQueryFormat){
				this.dateTimeQueryFormat = dateTimeQueryFormat;
			}


		public string ParseFilter(Expression expr){
				try {
					Visit (expr);
					return sb.ToString ();
				} catch (Exception ex) {
					Debug.WriteLine (ex.Message);
					return string.Empty;
				}
			}


			protected virtual Expression Visit(Expression exp) {
				if (exp == null)
					return exp;
				switch (exp.NodeType) {
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.ArrayLength:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
					return VisitUnary ((UnaryExpression)exp);
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.Divide:
				case ExpressionType.Modulo:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.Coalesce:
				case ExpressionType.ArrayIndex:
				case ExpressionType.RightShift:
				case ExpressionType.LeftShift:
				case ExpressionType.ExclusiveOr:
					return VisitBinary ((BinaryExpression)exp);
				case ExpressionType.Constant:
					return VisitConstant ((ConstantExpression)exp);
				case ExpressionType.Lambda:
					return VisitLambda ((LambdaExpression)exp);
				case ExpressionType.MemberAccess:
					return VisitMemberAccess ((MemberExpression)exp);
				default:
					throw new Exception (string.Format ("Unhandled expression type: '{0}'", exp.NodeType));
				}
			}



			protected virtual Expression VisitLambda(LambdaExpression lambda) {
				Expression body = Visit (lambda.Body);
				return body != lambda.Body ? Expression.Lambda (lambda.Type, body, lambda.Parameters) : lambda;
			}


			protected Expression VisitConstant (ConstantExpression node)
			{
				if (node.Value is string) {
					sb.Append (string.Format ("'{0}'", node.Value));
				} else {
					sb.Append (node.Value);
				}
				return node;
			}

			protected virtual Expression VisitUnary(UnaryExpression u) {
				Expression operand = Visit (u.Operand);
				return operand != u.Operand ? Expression.MakeUnary (u.NodeType, operand, u.Type, u.Method) : u;
			}

			protected virtual Expression VisitMemberAccess(MemberExpression m) {
				if (m.Expression!=null && m.Expression.Type == typeof(T)) {
					sb.Append (MemberNameToSearchName(m));
				}else {
					var value = Expression.Lambda (m).Compile ().DynamicInvoke ();
					if (value is DateTime) {
						sb.Append (string.Format ("'{0}'", ((DateTime)value).ToString (dateTimeQueryFormat)));
					} else if (value is DateTime?) {
						var d = ((DateTime?)value);
						if (d.HasValue) {
							sb.Append (string.Format ("'{0}'", d.Value.ToString (dateTimeQueryFormat)));
						}
					} else {
						sb.Append (string.Format ("'{0}'", value));
					}
				}
				return m;
			}

			protected Expression VisitBinary(BinaryExpression b) {
				Visit (b.Left);
				switch (b.NodeType) {
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					sb.Append (" AND ");
					break;
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					sb.Append (" OR ");
					break;
				case ExpressionType.Equal:
					sb.Append (" = ");
					break;
				case ExpressionType.NotEqual:
					sb.Append (" <> ");
					break;
				case ExpressionType.LessThan:
					sb.Append (" < ");
					break;
				case ExpressionType.LessThanOrEqual:
					sb.Append (" <= ");
					break;
				case ExpressionType.GreaterThan:
					sb.Append (" > ");
					break;
				case ExpressionType.GreaterThanOrEqual:
					sb.Append (" >= ");
					break;
				default:
					throw new NotSupportedException (string.Format ("The binary operator '{0}' is not supported", b.NodeType));
				}
				Visit (b.Right);
				return b;
			}
		}



	}
}
