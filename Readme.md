# AF Blog 后端项目

docker运行
```shell
docker run --name afblog0.5 -d -e MAF_MYSQL_CONN='你的连接字符串'  maf12138/backendwebapi:0.5
```

或者本地运行

需要配置环境变量

MAF_MYSQL_CONN

所连接的数据库中需要有T_Configs这张表

T_Configs存储了一些配置,具体查看项目启动入口Program注册的配置

前端参考

[zhiyiYo/KilaKila-Blog: 一个基于 SpringBoot 和 Vue3 的博客系统 (github.com)](https://github.com/zhiyiYo/KilaKila-Blog)
