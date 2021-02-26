﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FurionTemplate.Common.Helper
{
    public static class EnumConvert
    {

        public static string[] ToNameArray<T>()
        {
            return Enum.GetNames(typeof(T)).ToArray();
        }

        public static Array ToValueArray<T>()
        {
            return Enum.GetValues(typeof(T));
        }

        public static List<T> ToListOfValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }


        public static IEnumerable<T> ToEnumerable<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

    }
}
