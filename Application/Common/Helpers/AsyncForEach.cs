﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public static class AsyncForeach
    {
        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }
    }
}