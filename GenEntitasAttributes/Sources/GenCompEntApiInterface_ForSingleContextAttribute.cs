using System;

namespace GenEntitas
{

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = true)]
public class GenCompEntApiInterface_ForSingleContextAttribute : Attribute
{
	public GenCompEntApiInterface_ForSingleContextAttribute()
	{
	}
}

}

