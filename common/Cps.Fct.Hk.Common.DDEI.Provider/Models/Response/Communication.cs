namespace Cps.Fct.Hk.Common.DDEI.Provider.Models.Response;

using System.Text.Json.Serialization;

/// <summary>
/// Represents a communication related to a case, containing details such as the original file name,
/// document type, status, and classification.
/// </summary>
/// <param name="Id">The unique ID of the communication.</param>///
/// <param name="OriginalFileName">The original file name of the communication.</param>
/// <param name="Subject">The subject or title of the communication.</param>
/// <param name="DocumentTypeId">The document type ID of the communication, used to identify the document type.</param>
/// <param name="MaterialId">The unique material ID of the communication.</param>
/// <param name="Link">The downloadable link to the communication.</param>
/// <param name="Status">The current status of the communication (e.g., Used, None, Unused).</param>
/// <param name="Category">The category of the communication (e.g., MG Form, Statement).</param>
/// <param name="Type">The document type of the communication, mapped from the document type ID.</param>
/// <param name="HasAttachments">The flag used to identify if the communication has attachments.</param>
public record Communication(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("originalFileName")] string OriginalFileName,
    [property: JsonPropertyName("subject")] string Subject,
    [property: JsonPropertyName("documentTypeId")] int DocumentTypeId,
    [property: JsonPropertyName("materialId")] int MaterialId,
    [property: JsonPropertyName("link")] string Link,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("hasAttachments")] bool HasAttachments);
