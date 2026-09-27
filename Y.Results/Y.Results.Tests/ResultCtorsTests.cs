namespace Y.Results.Tests;

public class ResultCtorsTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void TestSuccessEmpty()
    {
        var result = Result.Success();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }
}