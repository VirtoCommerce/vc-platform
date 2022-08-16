using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Core.Json;

public class OutputJsonSerializerSettings : JsonSerializerSettings
{
    public virtual void CopyFrom(JsonSerializerSettings settings)
    {
        ReferenceLoopHandling = settings.ReferenceLoopHandling;
        MissingMemberHandling = settings.MissingMemberHandling;
        ObjectCreationHandling = settings.ObjectCreationHandling;
        NullValueHandling = settings.NullValueHandling;
        DefaultValueHandling = settings.DefaultValueHandling;
        Converters = settings.Converters;
        PreserveReferencesHandling = settings.PreserveReferencesHandling;
        TypeNameHandling = settings.TypeNameHandling;
        MetadataPropertyHandling = settings.MetadataPropertyHandling;
        TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
        ConstructorHandling = settings.ConstructorHandling;
        ContractResolver = settings.ContractResolver;
        EqualityComparer = settings.EqualityComparer;
        ReferenceResolverProvider = settings.ReferenceResolverProvider;
        TraceWriter = settings.TraceWriter;
        SerializationBinder = settings.SerializationBinder;
        Error = settings.Error;
        Context = settings.Context;
        DateFormatString = settings.DateFormatString;
        MaxDepth = settings.MaxDepth;
        Formatting = settings.Formatting;
        DateFormatHandling = settings.DateFormatHandling;
        DateTimeZoneHandling = settings.DateTimeZoneHandling;
        DateParseHandling = settings.DateParseHandling;
        FloatFormatHandling = settings.FloatFormatHandling;
        FloatParseHandling = settings.FloatParseHandling;
        StringEscapeHandling = settings.StringEscapeHandling;
        Culture = settings.Culture;
        CheckAdditionalContent = settings.CheckAdditionalContent;
    }
}
