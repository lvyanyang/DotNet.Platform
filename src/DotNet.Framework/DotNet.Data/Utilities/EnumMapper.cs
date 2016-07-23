// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet.Utility;

namespace DotNet.Data.Utilities
{
	internal static class EnumMapper
	{
	    static Cache<Type, Dictionary<string, object>> _types = new Cache<Type,Dictionary<string,object>>();

	    public static object EnumFromString(Type enumType, string value)
		{
			Dictionary<string, object> map = _types.Get(enumType, () =>
			{
				var values = Enum.GetValues(enumType);

				var newmap = new Dictionary<string, object>(values.Length, StringComparer.InvariantCultureIgnoreCase);

				foreach (var v in values)
				{
					newmap.Add(v.ToString(), v);
				}

				return newmap;
			});


			return map[value];
		}
	}
}
