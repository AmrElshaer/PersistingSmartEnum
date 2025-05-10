using Ardalis.SmartEnum;

namespace PersistingSmartEnum.Enums;


public  class LanguageEnum(string name,int value):SmartEnum<LanguageEnum>(name,value)
{
    
    public static readonly LanguageEnum Arabic = new ArabicLanguage();
    public static readonly LanguageEnum English = new EnglishLanguage();

    private sealed class ArabicLanguage() : LanguageEnum("ar", 1);

    private sealed class EnglishLanguage() : LanguageEnum("en", 2);
}