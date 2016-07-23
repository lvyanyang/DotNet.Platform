// ===============================================================================
// DotNet.Platform 开发框架 2016 版权所有
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Linq.Expressions;
using DotNet.Entity;
using DotNet.Utility;

namespace DotNet.Data.Utilities
{
    class PocoData
    {
        static Cache<Type, PocoData> _pocoDatas = new Cache<Type, PocoData>();
        static List<Func<object, object>> _converters = new List<Func<object, object>>();
        static MethodInfo fnGetValue = typeof(IDataRecord).GetMethod("GetValue", new Type[] { typeof(int) });
        static MethodInfo fnIsDBNull = typeof(IDataRecord).GetMethod("IsDBNull");
        static FieldInfo fldConverters = typeof(PocoData).GetField("_converters", BindingFlags.Static | BindingFlags.GetField | BindingFlags.NonPublic);
        static MethodInfo fnListGetItem = typeof(List<Func<object, object>>).GetProperty("Item").GetGetMethod();
        static MethodInfo fnInvoke = typeof(Func<object, object>).GetMethod("Invoke");

        Cache<Tuple<string, string, int, int>, Delegate> PocoFactories = new Cache<Tuple<string, string, int, int>, Delegate>();

        public PocoData()
        {
        }

        public PocoData(Type t)
        {
            Metadata = EntityMetadata.ForType(t);
        }

        public Type EntityType
        {
            get { return Metadata.EntityType; }
        }

        public string[] QueryColumns
        {
            get { return Metadata.QueryColumns; }
        }

        public TableInfo TableInfo
        {
            get { return Metadata.TableInfo; }
        }

        public Dictionary<string, EntityColumn> Columns
        {
            get { return Metadata.Columns; }
        }

        public EntityMetadata Metadata { get; private set; }

        public static PocoData ForType(Type t)
        {
            return _pocoDatas.Get(t, () => new PocoData(t));
        }

        static bool IsIntegralType(Type t)
        {
            var tc = Type.GetTypeCode(t);
            return tc >= TypeCode.SByte && tc <= TypeCode.UInt64;
        }

        public Delegate GetFactory(string sql, string connString, int firstColumn, int countColumns, IDataReader r)
        {
            // Check cache
            var key = Tuple.Create<string, string, int, int>(sql, connString, firstColumn, countColumns);

            return PocoFactories.Get(key, () =>
                {
                    // Create the method
                    var m = new DynamicMethod("petapoco_factory_" + PocoFactories.Count.ToString(), EntityType, new Type[] { typeof(IDataReader) }, true);
                    var il = m.GetILGenerator();
                    if (EntityType == typeof(object))
                    {
                        // var poco=new T()
                        il.Emit(OpCodes.Newobj, typeof(System.Dynamic.ExpandoObject).GetConstructor(Type.EmptyTypes));			// obj

                        MethodInfo fnAdd = typeof(IDictionary<string, object>).GetMethod("Add");

                        // Enumerate all fields generating a set assignment for the column
                        for (int i = firstColumn; i < firstColumn + countColumns; i++)
                        {
                            var srcType = r.GetFieldType(i);

                            il.Emit(OpCodes.Dup);						// obj, obj
                            il.Emit(OpCodes.Ldstr, r.GetName(i));		// obj, obj, fieldname

                            // Get the converter
                            //Func<object, object> converter = null;

                            /*
                            if (ForceDateTimesToUtc && converter == null && srcType == typeof(DateTime))
                                converter = delegate(object src) { return new DateTime(((DateTime)src).Ticks, DateTimeKind.Utc); };
                             */

                            // Setup stack for call to converter
                            //AddConverterToStack(il, converter);

                            // r[i]
                            il.Emit(OpCodes.Ldarg_0);					// obj, obj, fieldname, converter?,    rdr
                            il.Emit(OpCodes.Ldc_I4, i);					// obj, obj, fieldname, converter?,  rdr,i
                            il.Emit(OpCodes.Callvirt, fnGetValue);		// obj, obj, fieldname, converter?,  value

                            // Convert DBNull to null
                            il.Emit(OpCodes.Dup);						// obj, obj, fieldname, converter?,  value, value
                            il.Emit(OpCodes.Isinst, typeof(DBNull));	// obj, obj, fieldname, converter?,  value, (value or null)
                            var lblNotNull = il.DefineLabel();
                            il.Emit(OpCodes.Brfalse_S, lblNotNull);		// obj, obj, fieldname, converter?,  value
                            il.Emit(OpCodes.Pop);						// obj, obj, fieldname, converter?
                            //if (converter != null)
                            //    il.Emit(OpCodes.Pop);					// obj, obj, fieldname, 
                            il.Emit(OpCodes.Ldnull);					// obj, obj, fieldname, null
                            //if (converter != null)
                            //{
                            //    var lblReady = il.DefineLabel();
                            //    il.Emit(OpCodes.Br_S, lblReady);
                            //    il.MarkLabel(lblNotNull);
                            //    il.Emit(OpCodes.Callvirt, fnInvoke);
                            //    il.MarkLabel(lblReady);
                            //}
                            //else
                            {
                                il.MarkLabel(lblNotNull);
                            }

                            il.Emit(OpCodes.Callvirt, fnAdd);
                        }
                    }
                    else
                        if (EntityType.IsValueType || EntityType == typeof(string) || EntityType == typeof(byte[]))
                    {
                        // Do we need to install a converter?
                        var srcType = r.GetFieldType(0);
                        var converter = GetConverter(null, srcType, EntityType);

                        // "if (!rdr.IsDBNull(i))"
                        il.Emit(OpCodes.Ldarg_0);                                       // rdr
                        il.Emit(OpCodes.Ldc_I4_0);                                      // rdr,0
                        il.Emit(OpCodes.Callvirt, fnIsDBNull);                          // bool
                        var lblCont = il.DefineLabel();
                        il.Emit(OpCodes.Brfalse_S, lblCont);
                        il.Emit(OpCodes.Ldnull);                                        // null
                        var lblFin = il.DefineLabel();
                        il.Emit(OpCodes.Br_S, lblFin);

                        il.MarkLabel(lblCont);

                        // Setup stack for call to converter
                        AddConverterToStack(il, converter);

                        il.Emit(OpCodes.Ldarg_0);                                       // rdr
                        il.Emit(OpCodes.Ldc_I4_0);                                      // rdr,0
                        il.Emit(OpCodes.Callvirt, fnGetValue);                          // value

                        // Call the converter
                        if (converter != null)
                            il.Emit(OpCodes.Callvirt, fnInvoke);

                        il.MarkLabel(lblFin);
                        il.Emit(OpCodes.Unbox_Any, EntityType);                             // value converted
                    }
                    else
                    {
                        // var poco=new T()
                        il.Emit(OpCodes.Newobj, EntityType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null));

                        // Enumerate all fields generating a set assignment for the column
                        for (int i = firstColumn; i < firstColumn + countColumns; i++)
                        {
                            // Get the PocoColumn for this db column, ignore if not known
                            EntityColumn pc;
                            if (!Columns.TryGetValue(r.GetName(i), out pc))
                                continue;

                            // Get the source type for this column
                            var srcType = r.GetFieldType(i);
                            var dstType = pc.Property.Property.PropertyType;

                            // "if (!rdr.IsDBNull(i))"
                            il.Emit(OpCodes.Ldarg_0);                                       // poco,rdr
                            il.Emit(OpCodes.Ldc_I4, i);                                     // poco,rdr,i
                            il.Emit(OpCodes.Callvirt, fnIsDBNull);                          // poco,bool
                            var lblNext = il.DefineLabel();
                            il.Emit(OpCodes.Brtrue_S, lblNext);                             // poco

                            il.Emit(OpCodes.Dup);                                           // poco,poco

                            // Do we need to install a converter?
                            var converter = GetConverter(pc, srcType, dstType);

                            // Fast
                            bool Handled = false;
                            if (converter == null)
                            {
                                var valuegetter = typeof(IDataRecord).GetMethod("Get" + srcType.Name, new Type[] { typeof(int) });
                                if (valuegetter != null
                                        && valuegetter.ReturnType == srcType
                                        && (valuegetter.ReturnType == dstType || valuegetter.ReturnType == Nullable.GetUnderlyingType(dstType)))
                                {
                                    il.Emit(OpCodes.Ldarg_0);                                       // *,rdr
                                    il.Emit(OpCodes.Ldc_I4, i);                                     // *,rdr,i
                                    il.Emit(OpCodes.Callvirt, valuegetter);                         // *,value

                                    // Convert to Nullable
                                    if (Nullable.GetUnderlyingType(dstType) != null)
                                    {
                                        il.Emit(OpCodes.Newobj, dstType.GetConstructor(new Type[] { Nullable.GetUnderlyingType(dstType) }));
                                    }

                                    il.Emit(OpCodes.Callvirt, pc.Property.Property.GetSetMethod(true));      // poco
                                    Handled = true;
                                }
                            }

                            // Not so fast
                            if (!Handled)
                            {
                                // Setup stack for call to converter
                                AddConverterToStack(il, converter);

                                // "value = rdr.GetValue(i)"
                                il.Emit(OpCodes.Ldarg_0);                                       // *,rdr
                                il.Emit(OpCodes.Ldc_I4, i);                                     // *,rdr,i
                                il.Emit(OpCodes.Callvirt, fnGetValue);                          // *,value

                                // Call the converter
                                if (converter != null)
                                    il.Emit(OpCodes.Callvirt, fnInvoke);

                                // Assign it
                                il.Emit(OpCodes.Unbox_Any, pc.Property.Property.PropertyType);       // poco,poco,value
                                il.Emit(OpCodes.Callvirt, pc.Property.Property.GetSetMethod(true));      // poco
                            }

                            il.MarkLabel(lblNext);
                        }

                        var fnOnLoaded = RecurseInheritedTypes<MethodInfo>(EntityType, (x) => x.GetMethod("OnLoaded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null));
                        if (fnOnLoaded != null)
                        {
                            il.Emit(OpCodes.Dup);
                            il.Emit(OpCodes.Callvirt, fnOnLoaded);
                        }
                    }

                    il.Emit(OpCodes.Ret);

                    // Cache it, return it
                    return m.CreateDelegate(Expression.GetFuncType(typeof(IDataReader), EntityType));
                }
            );
        }

        private static void AddConverterToStack(ILGenerator il, Func<object, object> converter)
        {
            if (converter != null)
            {
                // Add the converter
                int converterIndex = _converters.Count;
                _converters.Add(converter);

                // Generate IL to push the converter onto the stack
                il.Emit(OpCodes.Ldsfld, fldConverters);
                il.Emit(OpCodes.Ldc_I4, converterIndex);
                il.Emit(OpCodes.Callvirt, fnListGetItem);					// Converter
            }
        }

        private static Func<object, object> GetConverter(EntityColumn pc, Type srcType, Type dstType)
        {
            //// Standard DateTime->Utc mapper
            //if (pc != null && pc.ForceToUtc && srcType == typeof(DateTime) && (dstType == typeof(DateTime) || dstType == typeof(DateTime?)))
            //{
            //    return src => new DateTime(((DateTime) src).Ticks, DateTimeKind.Utc);
            //}

            // Forced type conversion including integral types -> enum
            
            if (dstType.IsEnum && IsIntegralType(srcType))
            {
                if (srcType != typeof(int))
                {
                    return src => Convert.ChangeType(src, typeof(int), null);
                }
            }
            else if (!dstType.IsAssignableFrom(srcType))
            {
                if (dstType.IsEnum && srcType == typeof(string))
                {
                    return src => EnumMapper.EnumFromString(dstType, (string)src);
                }
                else
                {
                    return src =>
                    {
                        Debug.WriteLine($"GetConverter,Column ={pc?.ColumnName}");
                        return Convert.ChangeType(src, dstType, null);
                    };
                }
            }

            return null;
        }

        static T RecurseInheritedTypes<T>(Type t, Func<Type, T> cb)
        {
            while (t != null)
            {
                T info = cb(t);
                if (info != null)
                    return info;
                t = t.BaseType;
            }
            return default(T);
        }

        internal static void ClearCaches()
        {
            _pocoDatas.Clear();
        }

        /// <summary>
        /// 获取实体主键值
        /// </summary>
        /// <param name="poco">实体对象</param>
        /// <returns>返回实体主键值</returns>
        public object GetPrimaryKeyValue(object poco)
        {
            return Metadata.GetPrimaryKeyValue(poco);
        }

        /// <summary>
        /// 获取实体主键值
        /// </summary>
        /// <param name="poco">实体对象</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns>返回实体主键值</returns>
        public void SetPrimaryKeyValue(object poco, object primaryKeyValue)
        {
            Metadata.SetPrimaryKeyValue(poco, primaryKeyValue);
        }
    }
}
