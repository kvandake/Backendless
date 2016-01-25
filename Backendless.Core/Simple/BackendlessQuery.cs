using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Backendless.Core
{
	public class BackendlessQuery<T> : IBackendlessQuery where T: BackendlessEntity
	{

		public static BackendlessQuery<T> FirstQuery{
			get{
				return null;
			}
		}

		public static BackendlessQuery<T> LastQuery{
			get{
				return null;
			}
		}

		public static BackendlessQuery<T> AllCollectionQuery{
			get{
				return null;
			}
		}

		public static BackendlessQuery<T> FromObjectIdQuery(string objectId){
			return null;
		}



		readonly StringBuilder sb = new StringBuilder ();
		string dateTimeQueryFormat = "o";
		List<string> _props;
		List<string> _sortProps;
		Expression<Func<T,bool>> _filter;
		int? _pageSize;
		uint _offset;


		public string DateTimeQueryFormat {
			get {
				return dateTimeQueryFormat;
			}
			set {
				dateTimeQueryFormat = value;
			}
		}

		internal List<string> PropsInternal {
			get {
				return _props ?? (_props = new List<string> ());
			}
		}

		internal List<string> SortPropsInternal {
			get {
				return _sortProps ?? (_sortProps = new List<string> ());
			}
		}

		internal Expression<Func<T, bool>> FilterInternal {
			get {
				return _filter;
			}
		}

		internal int? PageSizeInternal {
			get {
				return _pageSize;
			}
		}

		internal uint OffsetInternal {
			get {
				return _offset;
			}
		}

		#region IBackendlessQuery implementation

		public string ToQuery {
			get {
				if (PropsInternal.Count != 0) {
					sb.Append (string.Format ("{0}={1}", "props", string.Join (",", PropsInternal)));
					sb.Append ("&");
				}
				if (FilterInternal != null) {
					sb.Append (ParseFilterProvider.Parse (FilterInternal,DateTimeQueryFormat));
					sb.Append ("&");
				} 
				if (PageSizeInternal.HasValue) {
					sb.Append (string.Format ("{0}={1}", "pageSize", PageSizeInternal));
					sb.Append ("&");
				}
				if (SortPropsInternal.Count != 0) {
					sb.Append (string.Format ("{0}={1}", "sortBy", string.Join (",", SortPropsInternal)));
					sb.Append ("&");
				}
				sb.Append (string.Format ("{0}={1}", "offset", OffsetInternal));
				return sb.ToString ();
			}
		}

		#endregion

		public BackendlessQuery<T> Prop<P>(Expression<Func<T,P>> prop){
			var member = prop.Body as MemberExpression;
			PropsInternal.Add (MemberNameToSearchName(member));
			return this;
		}

		public BackendlessQuery<T> Where(Expression<Func<T,bool>> filter){
			_filter = filter;
			return this;
		}

		public BackendlessQuery<T> SortBy<P>(Expression<Func<T,P>> prop){
			var member = prop.Body as MemberExpression;
			SortPropsInternal.Add (MemberNameToSearchName(member));
			return this;
		}

		public BackendlessQuery<T> PageSize(int pageSize){
			_pageSize = pageSize;
			return this;
		}

		public BackendlessQuery<T> Offset(uint offset){
			_offset = offset;
			return this;
		}
			
			




		#region VisitFilter



		#endregion

		class ParseFilterProvider {

		string dateTimeQueryFormat;
		readonly StringBuilder sb = new StringBuilder("where=");
		
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
					sb.Append (" OR");
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


		static string MemberNameToSearchName(MemberExpression member){
			var customAttributes = member.Member.CustomAttributes.ToList ();
			var def = customAttributes.FirstOrDefault (x => x.AttributeType == typeof(JsonPropertyAttribute));
			return def != null ? def.ConstructorArguments [0].Value.ToString () : member.Member.Name;
		}

	}
}

