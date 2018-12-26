using GraphQL.Types;
public class HelloWorldQuery : ObjectGraphType
{
    public HelloWorldQuery()
    {
        Field<StringGraphType>(
        name: "name",
        resolve: context => "Hacer"
        );
        Field<StringGraphType>(
        name: "surname",
        resolve: context => "Bakirci"
        );
    }
}