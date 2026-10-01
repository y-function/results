namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    [Test]
    public void TestThenNullResult()
    {
        var result = (Result)null!;
        Assert.Throws<ArgumentNullException>(() => result!.Then(() => { }));
    }
}