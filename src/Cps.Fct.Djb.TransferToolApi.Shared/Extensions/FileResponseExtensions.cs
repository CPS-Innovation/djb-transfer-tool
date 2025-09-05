// <copyright file="FileResponseExtensions.cs" company="TheCrownProsecutionService">
// Copyright (c) The Crown Prosecution Service. All rights reserved.
// </copyright>

namespace Cps.Fct.Djb.TransferToolApi.Shared.Extensions;

using System.Text.RegularExpressions;
using Cps.MasterDataService.Infrastructure.ApiClient;

public static class FileResponseExtensions
{
    /// <summary>
    /// Extracts the file name from the FileResponse headers.
    /// </summary>
    /// <param name="fileResponse">FileResponse.</param>
    /// <returns>The filename including extension.</returns>
    public static string GetFileName(this FileResponse fileResponse)
    {
        if (fileResponse.Headers.TryGetValue("X-File-Name", out var fileNameHeader))
        {
            var fileName = fileNameHeader.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return fileName;
            }
        }

        if (fileResponse.Headers.TryGetValue("Content-Disposition", out var contentDisposition))
        {
            var dispositionValue = contentDisposition.FirstOrDefault();
            if (!string.IsNullOrEmpty(dispositionValue))
            {
                var match = Regex.Match(dispositionValue, @"filename=""([^""]+)""", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }
        }

        return $"downloaded_file_{DateTime.Now:yyyyMMdd_HHmmss}.bin";
    }

    /// <summary>
    /// Extracts the content type from the FileResponse headers.
    /// </summary>
    /// <param name="fileResponse">FileResponse.</param>
    /// <returns>The content type of the file.</returns>
    public static string GetContentType(this FileResponse fileResponse)
    {
        if (fileResponse.Headers.TryGetValue("X-Content-Type", out var customContentType))
        {
            var contentType = customContentType.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                return contentType;
            }
        }

        if (fileResponse.Headers.TryGetValue("Content-Type", out var standardContentType))
        {
            var contentType = standardContentType.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                return contentType;
            }
        }

        return "application/octet-stream";
    }

    /// <summary>
    /// Saves the file from the FileResponse to the specified directory with an optional file name.
    /// </summary>
    /// <param name="fileResponse">FileResponse.</param>
    /// <param name="directoryPath">string.</param>
    /// <param name="fileName">string.</param>
    public static async Task SaveToFileAsync(this FileResponse fileResponse, string directoryPath, string? fileName = null)
    {
        fileName ??= fileResponse.GetFileName();
        string fullFilePath = Path.Combine(directoryPath, fileName);

        using var stream = fileResponse.Stream;
        if (stream.CanSeek && stream.Position != 0)
        {
            stream.Position = 0;
        }

        using var fileStream = File.Create(fullFilePath);
        await stream.CopyToAsync(fileStream).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns the file content from the FileResponse as a byte array.
    /// </summary>
    /// <param name="fileResponse">FileResponse.</param>
    /// <returns>A byte array.</returns>
    public static async Task<byte[]> ToByteArrayAsync(this FileResponse fileResponse)
    {
        using var stream = fileResponse.Stream;
        if (stream.CanSeek && stream.Position != 0)
        {
            stream.Position = 0;
        }

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
        return memoryStream.ToArray();
    }
}
