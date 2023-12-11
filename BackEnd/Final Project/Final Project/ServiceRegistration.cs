namespace Final_Project
{
    public static class ServiceRegistration
    {
        public static void Registration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
        }
    }
}
