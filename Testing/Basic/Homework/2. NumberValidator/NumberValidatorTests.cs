using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, Description = "Отрицательная длина целой части")]
    [TestCase(-1, 2, false, Description = "Отрицательная длина целой части без знака")]
    public void Constructor_WithInvalidIntegerLength_ShouldThrowArgumentException(
        int precision, int scale, bool onlyPositive)
    {
        Assert.Throws<ArgumentException>(() =>
            new NumberValidator(precision, scale, onlyPositive));
    }

    [TestCase(1, 0, true, Description = "Валидные параметры со знаком")]
    [TestCase(17, 2, true, Description = "Большие значения параметров")]
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
}