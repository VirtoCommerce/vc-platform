using System;

namespace VirtoCommerce.Platform.Core.Swagger;

/// <summary>
/// Marks an action as a <b>file upload endpoint that uses streaming</b>,
/// so that Swagger / OpenAPI generators describe a <c>multipart/form-data</c>
/// request body with a binary file field.
/// <para>
/// This attribute is intended for large-file uploads that read directly from
/// <c>HttpContext.Request.Body</c> (for example, with <c>MultipartReader</c>)
/// and typically use <c>[DisableFormValueModelBinding]</c>, rather than
/// binding <c>IFormFile</c> parameters.
/// </para>
/// <para>
/// It is consumed by platform-level Swagger / OpenAPI filters/transformers
/// (for both Swashbuckle and <c>Microsoft.AspNetCore.OpenApi</c>) and does
/// not change runtime behavior of the action itself.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class UploadFileAttribute : Attribute
{
    /// <summary>
    /// Logical name of the file field in the generated OpenAPI schema.
    /// Defaults to <c>"uploadedFile"</c>. This name is used only for
    /// documentation/UI (for example, Swagger UI form field name) and does
    /// not affect how the stream is read in the action.
    /// </summary>
    public string Name { get; set; } = "file";

    /// <summary>
    /// Human‑readable description for the file field in the generated
    /// OpenAPI document (for example, tooltip in Swagger UI).
    /// </summary>
    public string Description { get; set; } = "Upload File";

    /// <summary>
    /// OpenAPI schema type for the file property.
    /// For file uploads this should remain <c>"string"</c>; the corresponding
    /// schema formatter will set <c>format = "binary"</c> to indicate a
    /// streamed binary payload.
    /// See: https://swagger.io/docs/specification/v3_0/describing-request-body/file-upload/    
    /// </summary>
    public string Type { get; set; } = "string";

    /// <summary>
    /// Indicates whether the file field is required in the generated
    /// OpenAPI schema. Set to <c>true</c> when a file must always be
    /// provided in the multipart request body.
    /// </summary>
    public bool Required { get; set; } = false;

    /// <summary>
    /// When <c>true</c>, describes the file field as a collection of files
    /// (for example, an array of <c>string</c>/<c>binary</c> items) in the
    /// generated OpenAPI schema, allowing multiple files to be uploaded
    /// under the same logical field name.
    /// </summary>
    public bool AllowMultiple { get; set; } = false;
}
