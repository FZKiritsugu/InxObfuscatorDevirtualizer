using System.Collections.Generic;
using System.Reflection;



public class ValueStack
{
	public Stack<object> valueStack = new Stack<object>();

	public object[] locals;

	public Dictionary<FieldInfo, object> fields = new Dictionary<FieldInfo, object>();

	public object[] parameters;
}
