namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Request Parameter WithToken
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestParameterWithToken<T>
    {
        public T ParamData { get; set; }

        public Token Token { get; set; }
    }
}