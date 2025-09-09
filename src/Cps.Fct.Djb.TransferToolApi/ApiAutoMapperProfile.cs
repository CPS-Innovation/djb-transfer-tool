// <copyright file="ApiAutoMapperProfile.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi;

using AutoMapper;
using Cps.Fct.Djb.TransferToolApi.Models.Requests.Case;
using Cps.Fct.Djb.TransferToolApi.Models.Requests.Document;
using Cps.Fct.Djb.TransferToolApi.Models.Responses.Case;
using Cps.Fct.Djb.TransferToolApi.Models.Responses.Document;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Case;
using Cps.Fct.Djb.TransferToolApi.Shared.Dtos.CaseCenter.Document;
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

        this.CreateMap<string, CaseCreatedResponse>()
            .ConstructUsing(src => new CaseCreatedResponse
            {
                CaseCenterCaseId = src,
            });

        this.CreateMap<(UploadDocumentsFromCmsBundleRequest createCaseRequest, CmsAuthValues cmsAuthValues), UploadDocumentsFromCmsBundleDto>()
            .ConstructUsing(src => new UploadDocumentsFromCmsBundleDto
            {
                CmsCaseId = src.createCaseRequest.CmsCaseId,
                CmsBundleId = src.createCaseRequest.CmsBundleId,
                DocumentUploader = src.createCaseRequest.DocumentUploader,
                CmsClassicAuthCookies = src.cmsAuthValues.CmsCookies,
                CmsModernAuthToken = src.cmsAuthValues.CmsModernToken,
            });

        this.CreateMap<List<MultipleDocumentsUploadedFileDataDto>, DocumentsUploadedFromCmsBundleResponse>()
            .ConstructUsing(src => new DocumentsUploadedFromCmsBundleResponse
            {
                DocumentResponses = src.Select(dto => new DocumentUploadResponse
                {
                    CaseCenterDocumentId = dto.CaseCenterDocumentId,
                    CmsDocumentId = dto.CmsDocumentId,
                    IsUploaded = dto.UploadStatus,
                    ErrorMessage = dto.ErrorMessage,
                }).ToList(),
            });
    }
}
