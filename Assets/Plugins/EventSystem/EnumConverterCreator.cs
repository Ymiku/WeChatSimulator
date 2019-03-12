using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace graduation
{
    /// <summary>
    /// enum convert helper,use dynamic function and convert without boxing
    /// </summary>
    public static class EnumConverterCreator
    {
        //Creates and returns a dynamic function that converts an enum to the specified result type (such as an int)
        public static Func<TEnum, TResult> CreateFromEnumConverter<TEnum, TResult>()
                                                                                    where TEnum : struct
                                                                                    where TResult : struct
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));

            var dynam = new DynamicMethod("__" + typeof(TEnum).Name + "_to_" + typeof(TResult).Name, typeof(TResult),
                                          new[] { typeof(TEnum) }, true);
            ILGenerator il = dynam.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0, 0);
            int resultSize = Marshal.SizeOf(typeof(TResult));
            if (resultSize != Marshal.SizeOf(underlyingType))
                EmitConversionOpcode(il, resultSize);
            il.Emit(OpCodes.Ret);

            return (Func<TEnum, TResult>)dynam.CreateDelegate(typeof(Func<TEnum, TResult>));
        }

        //Creates and returns a dynamic function that converts an input (such as an int) to an enum
        public static Func<TInput, TEnum> CreateToEnumConverter<TInput, TEnum>()
                                                                                where TEnum : struct
                                                                                where TInput : struct
        {
            Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));

            var dynam = new DynamicMethod("__" + typeof(TInput).Name + "_to_" + typeof(TEnum).Name, typeof(TEnum),
                                          new[] { typeof(TInput) }, true);
            ILGenerator il = dynam.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0, 0);
            int enumSize = Marshal.SizeOf(underlyingType);
            if (enumSize != Marshal.SizeOf(typeof(TInput)))
                EmitConversionOpcode(il, enumSize);
            il.Emit(OpCodes.Ret);

            return (Func<TInput, TEnum>)dynam.CreateDelegate(typeof(Func<TInput, TEnum>));
        }

        //Helper stuff for the two methods above.
        private static readonly OpCode[] _converterOpCodes = new[] { OpCodes.Conv_I1, OpCodes.Conv_I2, OpCodes.Conv_I4, OpCodes.Conv_I8 };

        private static void EmitConversionOpcode(ILGenerator il, int resultSize)
        {
            if (resultSize <= 0)
                throw new ArgumentOutOfRangeException("resultSize", resultSize, "Result size must be a power of 2");
            int n = 0;
            while (true)
            {
                if (n >= _converterOpCodes.Length)
                    throw new ArgumentOutOfRangeException("resultSize", resultSize, "Invalid result size");
                if ((resultSize >> n) == 1)
                {
                    il.Emit(_converterOpCodes[n]);
                    return;
                }
                n++;
            }
        }
    }
}
