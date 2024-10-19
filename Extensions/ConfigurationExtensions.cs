using Microsoft.Extensions.Configuration;

namespace WebSite.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetUrl(this IConfiguration config)
        {
            return config["URL"]!;
        }

        public static string GetHomePage(this IConfiguration config)
        {
            return config["Views:Home"]!;
        }

        public static string GetCustomerSignInPage(this IConfiguration config)
        {
            return config["Views:Customer:SignIn"]!;
        }

        public static string GetCustomerSignUpPage(this IConfiguration config)
        {
            return config["Views:Customer:SignUp"]!;
        }

        public static string GetAdminPassword(this IConfiguration config)
        {
            return config["AdminPassword"]!;
        }

        public static string GetAdminLigin(this IConfiguration config)
        {
            return config["AdminLogin"]!;
        }

		public static string GetImagesPath(this IConfiguration config)
		{
			return config["ImagesPath"]!;
		}
	}
}
