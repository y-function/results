namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    private Exception ExpectedException { get; } = new("Expected exception.");
    
    [SetUp]
    public void Setup() { }

    private static void AssertConditionFailure(Result result)
    {
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<Exception>());
        Assert.That(result.Exception!.Message, Is.EqualTo("Condition validation has failed."));
    }
}