using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{

    [TestCase(0, 2, true, Description = "Precision равен нулю")]
    [TestCase(-5, 2, true, Description = "Precision отрицательный")]
    public void Constructor_WithInvalidPrecision_ShouldThrowArgumentException(
        int precision, int scale, bool onlyPositive)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new NumberValidator(precision, scale, onlyPositive));

        Assert.That(exception.Message, Does.Contain("precision must be a positive number"));
    }


    [TestCase(5, -1, true, Description = "Scale отрицательный")]
    [TestCase(2, 2, true, Description = "Scale равен Precision")]
    [TestCase(2, 5, true, Description = "Scale больше Precision")]
    public void Constructor_WithInvalidScale_ShouldThrowArgumentException(
        int precision, int scale, bool onlyPositive)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new NumberValidator(precision, scale, onlyPositive));

        Assert.That(exception.Message, Does.Contain("scale must be a non-negative number"));
    }


    [TestCase("+14.23", 5, 2, true, Description = "Положительное число со знаком")]
    [TestCase("-14.23", 5, 2, false, Description = "Отрицательное число со знаком")]
    public void isValidNumber_SignOfTheNumber_ShouldReturnTrue(
    string input,
    int precision,
    int scale,
    bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(true));
    }

    [TestCase("-0.00", 3, 2, true, Description = "Отрицательный ноль")]
    [TestCase("+0.00", 3, 2, true, Description = "Положительный ноль со знаком")]
    [TestCase("00.00", 3, 2, true,  Description = "Ведущий ноль в целой части")]
    [TestCase("+1.23", 3, 2, true,  Description = "Число со знаком, но недостаточная длина целой части")]
    public void IsValidNumber_ShouldReturnFalse(
        string input,
        int precision,
        int scale,
        bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(false));
    }

    [TestCase("0.0", 17, 2, true, Description = "Простой ноль с дробной частью")]
    [TestCase("0", 17, 2, true, Description = "Простой ноль без дробной части")]
    [TestCase("+1.23", 4, 2, true, Description = "Положительное число со знаком, достаточная длина")]
    public void IsValidNumber_ShouldReturnTrue(string input,
        int precision,
        int scale,
        bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(true));
    }


    [Test]
    public void IsValidNumber_SoMuchScaleSymbols_ShouldReturnFalse()
    {
        var validator = new NumberValidator(17, 2, true);

        Assert.That(validator.IsValidNumber("0.000"), Is.EqualTo(false));
    }

    [Test]
    public void IsValidNumber_IncorrectNumberSign_ShouldRetrunFalse()
    {
        var validator = new NumberValidator(3, 2, true);

        Assert.That(validator.IsValidNumber("-1.23"), Is.EqualTo(false));
    }

    [Test]
    public void IsValidNumber_LettersInsteadOfNumbers_ShouldRetrunFalse()
    {
        var validator = new NumberValidator(3, 2, true);

        Assert.That(validator.IsValidNumber("a.sd"), Is.EqualTo(false));
    }

    [TestCase("+1.2", 3, 1, true, true, Description = "Ровно на границе precision (3 символа)")]
    [TestCase("124.3", 3, 1, true, false, Description = "Превышение precision на 1 символ (4 символа)")]
    [TestCase("1.2", 3, 1, true, true, Description = "Меньше границы precision (без знака, 3 символа)")]
    public void IsValidNumber_BoundaryPrecision_ShouldReturnExpected(
    string input, int precision, int scale, bool onlyPositive, bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        Assert.That(validator.IsValidNumber(input), Is.EqualTo(expectedResult));
    }

    [Test]
    public void Constructor_ScaleEqualsPrecision_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(2, 2, true));
    }
}