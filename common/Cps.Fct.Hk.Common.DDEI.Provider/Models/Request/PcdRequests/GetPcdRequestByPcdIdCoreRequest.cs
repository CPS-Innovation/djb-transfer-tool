// <copyright file="GetPcdRequestsCoreRequest.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>
namespace Cps.Fct.Hk.Common.DDEI.Provider.Models.Request.PcdRequests;
using System;
using Cps.Fct.Hk.Common.DDEI.Client.Model;

/// <summary>
/// Get PCD request overview by PCD id and case id .
/// </summary>
/// <param name="caseId"></param>
///  <param name="pcdId"></param>
/// <param name="CorrespondenceId"></param>
public record GetPcdRequestByPcdIdCoreRequest(int caseId, int pcdId, Guid CorrespondenceId)
        : BaseRequest(CorrespondenceId: CorrespondenceId)
{
}
