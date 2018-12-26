using GraphQL.Types;
public class HelloWorldSchema : Schema
{
    public HelloWorldSchema(HelloWorldQuery query)
    {
        Query = query;
    }
}