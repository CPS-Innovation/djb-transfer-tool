// <copyright file="ApiAutoMapperProfile.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi;

using AutoMapper;
using Cps.Fct.Djb.TransferToolApi.Shared.Interfaces;

/// <summary>
/// AutoMapper profile for API layer mappings.
/// </summary>
public class ApiAutoMapperProfile : Profile, IAutoMapperDependencyScanner
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiAutoMapperProfile"/> class.
    /// </summary>
    public ApiAutoMapperProfile()
    {
    }
}
