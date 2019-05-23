﻿namespace JudgeSystem.Web.Infrastructure.Extensions
{
	using System;
	using System.Collections.Generic;

	public static class EnumExtensions
	{
		public static IEnumerable<string> GetEnumValuesAsString<T>()
		{
			foreach (T element in Enum.GetValues(typeof(T)))
			{
				yield return element.ToString();
			}
		}

		public static string FormatResourceType(this string resourceType)
		{
			switch (resourceType)
			{
				case "AuthorsSolution": return "Authors Solution";
				case "ProblemsDescription": return "Problems Description";
				default: return resourceType.ToString();
			}
		}
	}
}
