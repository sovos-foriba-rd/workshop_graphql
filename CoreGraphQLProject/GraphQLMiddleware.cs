using System;
using System.IO;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using CoreGraphQL;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
public class GraphQLMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDocumentWriter _writer;
    private readonly IDocumentExecuter _executer;
    private readonly ISchema _schema;
    public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executer, ISchema schema)
    {
        _next = next;
        _writer = writer;
        _executer = executer;
        _schema = schema;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Path.StartsWithSegments("/api/graphql") && string.Equals(httpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
        {
            string body;
            using (var streamReader = new StreamReader(httpContext.Request.Body))
            {
                body = await streamReader.ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<GraphQLRequest>(body);
                var result = await new DocumentExecuter().ExecuteAsync(doc =>
                {
                    doc.Schema = _schema;
                    doc.Query = request.Query;
                }).ConfigureAwait(false);
                var json = new DocumentWriter(indent: true).Write(result);
                await httpContext.Response.WriteAsync(json);
            }
        }
        else
        {
            await _next(httpContext);
        }
    }
}