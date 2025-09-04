// <copyright file="ApiAutoMapperProfile.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi;

using AutoMapper;
using Cps.Fct.Djb.TransferToolApi.Models.Requests.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Interfaces;
using Cps.Fct.Hk.Common.DDEI.Client.Model;

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
        this.CreateMap<(CreateCaseRequest createCaseRequest, CmsAuthValues cmsAuthValues), CreateCaseDto>()
            .ConstructUsing(src => new CreateCaseDto
            {
                CmsCaseId = src.createCaseRequest.CmsCaseId,
                CaseCreator = src.createCaseRequest.CmsUsername,
                CmsClassicAuthCookies = src.cmsAuthValues.CmsCookies,
                CmsModernAuthToken = src.cmsAuthValues.CmsModernToken,
            });
    }
}
