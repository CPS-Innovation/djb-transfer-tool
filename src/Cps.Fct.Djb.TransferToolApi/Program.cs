// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly
using Cps.Fct.Hk.Common.Functions;
using Cps.Fct.Djb.TransferToolApi;

#pragma warning restore SA1200 // Using directives should be placed correctly

CommonHostApi.Run<Startup>(
    args,
    preCreateHostBuilderHook: () => { },
    postCreateHostBuilderHook: builder => builder);
