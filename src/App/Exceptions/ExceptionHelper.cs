public static class ExceptionHelper
{

    // could make one dynamic but this is just because I dont want to add too many args all the time.
    public static void ThrowIfUsernameEmptyOrWhiteSpace(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Input cannot be empty or whitespace.", nameof(name));
    }

    public static void ThrowIfEmailEmptyOrWhiteSpace(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Input cannot be empty or whitespace.", nameof(name));
    }
    

    public static void ThrowIfZero(int value, string paramName)
    {
        if (value == 0)
            throw new ArgumentException("Value cannot be 0.", paramName);
    }

    public static void ThrowIfNegative(int value, string paramName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
    }

    public static void ThrowIfNegativeOrZero(int value, string paramName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than 0.");
    }
    
}
