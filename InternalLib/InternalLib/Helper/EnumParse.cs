/*########################################################
 *#  InternalLib.dll                                     #
 *#  Copyright 2018 by WesTex Enterprises                #
 *########################################################*/

using System;
using System.ComponentModel;
using System.Reflection;

namespace InternalLib
{
    public static class EnumParse
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string ParseEnum<T>(int id)
        {
            string result = "";
            T inEnum = (T)Enum.ToObject(typeof(T), id);
            result = inEnum.ToString();
            if(result.Match(id.ToString()))
            {
                result = "";
            }
            return result;
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());


            return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute) ? value.ToString() : attribute.Description;
        }
    }
}