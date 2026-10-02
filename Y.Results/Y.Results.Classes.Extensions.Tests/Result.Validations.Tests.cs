namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    [Test]
    public void TestThrowIfFailedForSuccess()
    {
        var input = Result.Success();
        Assert.DoesNotThrow(input.ThrowIfFailed);
    }

    [Test]
    public void TestThrowIfFailedForFailure()
    {
        var input = Result.Failure(ExpectedException);
        var e = Assert.Throws(ExpectedException.GetType(), input.ThrowIfFailed);
        Assert.That(e, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestGetValueOrThrowForSuccess()
    {
        object value = 1;
        var input = Result.Success(value);
        var output = input.GetValueOrThrow();
        Assert.That(output, Is.EqualTo(value));
    }

    [Test]
    public void TestGetValueOrThrowForFailure()
    {
        var input = Result.Failure<object>(ExpectedException);
        var e = Assert.Throws(ExpectedException.GetType(), () => input.GetValueOrThrow());
        Assert.That(e, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestValidateForNullResult()
    {
        var input = (Result)null!;
        Assert.Throws<ArgumentNullException>(() => input.Validate(() => true));
    }

    [Test]
    public void TestValidateForSuccessWithNullPredicate()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.Validate(null!));
    }

    [Test]
    public void TestValidateForFailureWithNullPredicate()
    {
        var input = Result.Failure(ExpectedException);
        Assert.Throws<ArgumentNullException>(() => input.Validate(null!));
    }

    [Test]
    public void TestValidateForSuccessWithTruePredicate()
    {
        var input = Result.Success();
        var output = input.Validate(() => true);
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestValidateForSuccessWithTruePredicateCustomError()
    {
        var input = Result.Success();
        var output = input.Validate(
            () => true,
            () =>
                {
                    Assert.Fail();
                    return ExpectedException;
                });
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestValidateForSuccessWithFalsePredicate()
    {
        var input = Result.Success();
        var output = input.Validate(() => false);
        Assert.That(output, Is.Not.SameAs(input));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.Not.Null);
        Assert.That(output.Exception, Is.TypeOf<Exception>());
        Assert.That(output.Exception!.Message, Is.EqualTo("Condition validation has failed."));
    }

    [Test]
    public void TestValidateForSuccessWithFalsePredicateCustomError()
    {
        var input = Result.Success();
        var output = input.Validate(() => false, () => ExpectedException);
        Assert.That(output, Is.Not.SameAs(input));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestValidateForFailureWithPredicate()
    {
        var input = Result.Failure(ExpectedException);
        var output = input.Validate(() =>
            {
                Assert.Fail();
                return false;
            });
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestValidateTypedForNullResult()
    {
        var input = (Result<int>)null!;
        Assert.Throws<ArgumentNullException>(() => input.Validate(() => true));
    }

    [Test]
    public void TestValidateTypedForSuccessWithNullPredicate()
    {
        var input = Result.Success(1);
        Assert.Throws<ArgumentNullException>(() => input.Validate(null!));
    }

    [Test]
    public void TestValidateTypedForFailureWithNullPredicate()
    {
        var input = Result.Failure<int>(ExpectedException);
        Assert.Throws<ArgumentNullException>(() => input.Validate(null!));
    }

    [Test]
    public void TestValidateTypedForSuccessWithTruePredicate()
    {
        var input = Result.Success(1);
        var output = input.Validate(_ => true);
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestValidateTypedForSuccessWithTruePredicateCustomError()
    {
        var input = Result.Success(1);
        var output = input.Validate(
            _ => true,
            () =>
                {
                    Assert.Fail();
                    return ExpectedException;
                });
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestValidateTypedForSuccessWithFalsePredicate()
    {
        var input = Result.Success(1);
        var output = input.Validate(_ => false);
        Assert.That(output, Is.Not.SameAs(input));
        AssertConditionFailure(output);
    }

    [Test]
    public void TestValidateTypedForSuccessWithFalsePredicateCustomError()
    {
        var input = Result.Success(1);
        var output = input.Validate(_ => false, () => ExpectedException);
        Assert.That(output, Is.Not.SameAs(input));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestValidateTypedForSuccessWithFalsePredicateCustomErrorTyped()
    {
        var input = Result.Success(1);
        var output = input.Validate(_ => false, v =>
            {
                Assert.That(v, Is.EqualTo(input.Value));
                return ExpectedException;
            });
        Assert.That(output, Is.Not.SameAs(input));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestValidateTypedForFailureWithPredicate()
    {
        var input = Result.Failure<int>(ExpectedException);
        var output = input.Validate(_ =>
            {
                Assert.Fail();
                return false;
            });
        Assert.That(output, Is.SameAs(input));
    }
    
    [Test]
    public void TestValidateNotNullForSuccessHavingNull()
    {
        var input = Result.Success<object>(null!);
        var output = input.ValidateNotNull();
        AssertConditionFailure(output);
    }

    [Test]
    public void TestValidateNotNullForSuccessHavingNullCustomError()
    {
        var input = Result.Success<object>(null!);
        var output = input.ValidateNotNull(() => ExpectedException);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestValidateNotNullForSuccessNotNull()
    {
        var input = Result.Success(new object());
        var output = input.ValidateNotNull();
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestValidateNotNullForFailure()
    {
        var input = Result.Failure<object>(ExpectedException);
        var output = input.ValidateNotNull();
        Assert.That(output, Is.SameAs(input));
    } 
}