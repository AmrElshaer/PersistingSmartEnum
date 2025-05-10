using PersistingSmartEnum.Enums;

namespace PersistingSmartEnum;

public interface ICurrentUserService
{
    public LanguageEnum Language { get; }
}

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    
    public LanguageEnum Language => LanguageEnum.FromName(httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString()
                                                                  ?.Split(',').FirstOrDefault()
                                                              ?? "en") ;
}