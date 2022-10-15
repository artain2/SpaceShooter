﻿using System;
using System.Collections.Generic;
using AppBootstrap.Injector.InjectUtils;

namespace AppBootstrap.Runtime.Injector.InjectUtils
{
    /// <summary>
    /// Info about single injectable item
    /// </summary>
    [Serializable]
    public class InjectingInfo
    {
        public string TypeName;
        public bool IsInAssembly;
        public List<CollectionInjectingOrder> CollectionsInjectingOrder =
            new List<CollectionInjectingOrder>();
        public List<RealizationInjectingInfo> Realizations = 
            new List<RealizationInjectingInfo>();

    }
}