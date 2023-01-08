using ApiManager._keenthemes.libs;

namespace ApiManager._keenthemes;

public interface IKTBootstrapBase
{
    void initThemeMode();
    
    void initThemeDirection();

    void initLayout();
    
    void init(IKTTheme theme);
}