﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.EventApi
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class HookAttribute : Attribute { }
}