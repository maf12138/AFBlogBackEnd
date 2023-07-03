namespace BackEndWebAPI.DTO;


//使用记录类型有问题啊,这里直接不可变类型了
//public record TagDTO(string name, int id);
public class TagDTO
{
    public string name { get; set; }

    public int id { get; set; }
}