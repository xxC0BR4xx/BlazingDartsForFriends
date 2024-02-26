using Blazor.IndexedDB.Framework;
using Classes;
using Microsoft.JSInterop;
using System;

namespace BlazingDartsForFriends.Classes
{
    public class indexedDB: IndexedDb
    {
        
            public indexedDB(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }
        
    }
}
