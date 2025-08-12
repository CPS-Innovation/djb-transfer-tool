// <copyright file="Program.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly

using Cps.Fct.Djb.TransferToolApi;
using Cps.Fct.Hk.Common.Functions;

#pragma warning restore SA1200 // Using directives should be placed correctly

CommonHostApi.Run<Startup>(
    args,
    preCreateHostBuilderHook: () => { },
    postCreateHostBuilderHook: builder => builder);
