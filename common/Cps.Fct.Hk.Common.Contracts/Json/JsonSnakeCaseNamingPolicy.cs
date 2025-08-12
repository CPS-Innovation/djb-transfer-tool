namespace Cps.Fct.Hk.Common.Contracts.Json;

using System.Text.Json;

/// <summary>Json policy to convert property names to snake case.</summary>
public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    /// <inheritdoc/>
    public override string ConvertName(string name)
    {
        return new string(ToSnakeCase(name.GetEnumerator()).ToArray());
    }

    private static IEnumerable<char> ToSnakeCase(CharEnumerator e)
    {
        if (!e.MoveNext())
        {
            yield break;
        }

        yield return char.ToLower(e.Current);
        while (e.MoveNext())
        {
            if (char.IsUpper(e.Current) || char.IsNumber(e.Current))
            {
                yield return '_';
                yield return char.ToLower(e.Current);
            }
            else
            {
                yield return e.Current;
            }
        }
    }
}
