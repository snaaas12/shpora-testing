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


    [TestCase(1, 0, true, Description = "Минимальные валидные параметры")]
    [TestCase(17, 2, true, Description = "Большие валидные параметры")]
    [TestCase(5, 4, true, Description = "Scale меньше Precision (валидно)")]
    public void Constructor_WithValidParameters_ShouldNotThrow(
        int precision, int scale, bool onlyPositive)
    {
        Assert.DoesNotThrow(() =>
            new NumberValidator(precision, scale, onlyPositive));
    }


    [TestCase("0.0", 17, 2, true, true, Description = "Простой ноль с дробной частью")]
    [TestCase("0", 17, 2, true, true, Description = "Простой ноль без дробной части")]
    [TestCase("+1.23", 4, 2, true, true, Description = "Положительное число со знаком, достаточная длина")]
    [TestCase("00.00", 3, 2, true, false, Description = "Ведущий ноль в целой части")]
    [TestCase("-0.00", 3, 2, true, false, Description = "Отрицательный ноль")]
    [TestCase("+0.00", 3, 2, true, false, Description = "Положительный ноль со знаком")]
    [TestCase("+1.23", 3, 2, true, false, Description = "Число со знаком, но недостаточная длина целой части")]
    public void IsValidNumber_ShouldReturnExpectedResult(
        string input,
        int precision,
        int scale,
        bool onlyPositive,
        bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(expectedResult));
    }


    [TestCase("0.000", 17, 2, true, false, Description = "Слишком много знаков после запятой")]
    public void IsValidNumber_SoMuchScaleSymbols_ShouldReturnFalse(string input,
        int precision,
        int scale,
        bool onlyPositive,
        bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(expectedResult));
    }

    [TestCase("-1.23", 3, 2, true, false, Description = "Отрицательное число (знак не разрешен)")]
    public void IsValidNumber_IncorrectNumberSign_ShouldRetrunFalse(string input,
        int precision,
        int scale,
        bool onlyPositive,
        bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(expectedResult));
    }

    [TestCase("a.sd", 3, 2, true, false, Description = "Некорректный формат")]
    public void IsValidNumber_LettersInsteadOfNumbers_ShouldRetrunFalse(string input,
        int precision,
        int scale,
        bool onlyPositive,
        bool expectedResult)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(expectedResult));
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
    [Description("Текущий код выбрасывает исключение, если scale == precision. Это ожидаемое поведение или баг?")]
    public void Constructor_ScaleEqualsPrecision_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new NumberValidator(2, 2, true));
    }
}