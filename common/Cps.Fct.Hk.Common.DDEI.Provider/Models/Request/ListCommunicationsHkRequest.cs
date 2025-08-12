// <copyright file="ListCommunicationsHkRequest.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Hk.Common.DDEI.Provider.Models.Request;

using System;
using Cps.Fct.Hk.Common.DDEI.Client.Model;

/// <summary>
/// The request to list communications.
/// </summary>
/// <param name="CaseId">The numeric case ID.</param>
/// <param name="CorrespondenceId">The correspondence ID.</param>
public record ListCommunicationsHkRequest(int CaseId, Guid CorrespondenceId)
        : BaseRequest(CorrespondenceId: CorrespondenceId)
{
}

