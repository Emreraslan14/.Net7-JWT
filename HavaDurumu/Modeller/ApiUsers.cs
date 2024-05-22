namespace HavaDurumu.Modeller
{
    public class ApiUsers
    {
        //Sadece öğrenme amaçlı, bu sınıfı bir veritabanıymış gibi kullanacağız.

        public static List<ApiUser> Users = new()
        {
            new ApiUser { Id = 1,Name="Emre",Password="123456",Role="Manager"},
            new ApiUser { Id = 2,Name="Ahmet",Password="123456",Role="Ordinary"}
        };
    }
}
