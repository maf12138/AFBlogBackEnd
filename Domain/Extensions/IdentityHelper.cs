
namespace Domain.Extensions;

public class IdentityHelper
{
    /// <summary>
    /// 返回生成的Token
    /// 参数为Token长度
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GenerateToken(int x)
    {
        var rand = new Random();
        var result = "";
        for (int i = 0; i < 6; i++)
        {
            result += rand.Next(0, 9).ToString();
        }
        return result;
    }
}
